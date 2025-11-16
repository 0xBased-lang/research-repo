# MediRemind: Technical Implementation Guide

**Last Updated:** 2025-11-16
**Target Audience:** Developers, Technical Decision Makers
**Tech Stack:** React Native + Firebase (Recommended)

---

## TABLE OF CONTENTS

1. [System Architecture](#1-system-architecture)
2. [Notification System (Critical)](#2-notification-system-critical)
3. [Database Schema](#3-database-schema)
4. [Authentication & Multi-User](#4-authentication--multi-user)
5. [Photo Storage & Optimization](#5-photo-storage--optimization)
6. [Offline-First Architecture](#6-offline-first-architecture)
7. [Security & Privacy](#7-security--privacy)
8. [Testing Strategy](#8-testing-strategy)
9. [Deployment & CI/CD](#9-deployment--cicd)
10. [Cost Optimization](#10-cost-optimization)
11. [Monitoring & Error Tracking](#11-monitoring--error-tracking)
12. [Scalability Considerations](#12-scalability-considerations)

---

## 1. SYSTEM ARCHITECTURE

### 1.1 High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     MOBILE APP (React Native)                │
├─────────────────────────────────────────────────────────────┤
│  Presentation Layer                                          │
│  ├─ Screens (Login, Dashboard, Add Medication, Timeline)    │
│  ├─ Components (MedicationCard, NotificationHandler)        │
│  └─ Navigation (React Navigation)                           │
├─────────────────────────────────────────────────────────────┤
│  Business Logic Layer                                        │
│  ├─ State Management (React Context + Hooks)                │
│  ├─ Medication Scheduling Engine                            │
│  ├─ Notification Manager                                    │
│  └─ Sync Controller (online/offline)                        │
├─────────────────────────────────────────────────────────────┤
│  Data Layer                                                  │
│  ├─ Firebase SDK (@react-native-firebase)                   │
│  ├─ Local Storage (AsyncStorage, SQLite)                    │
│  └─ Image Cache                                             │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                   FIREBASE BACKEND                           │
├─────────────────────────────────────────────────────────────┤
│  Authentication (Firebase Auth)                              │
│  ├─ Email/Password                                          │
│  ├─ OAuth (Google, Apple)                                   │
│  └─ Custom tokens (family sharing)                          │
├─────────────────────────────────────────────────────────────┤
│  Database (Cloud Firestore)                                  │
│  ├─ Users, Patients, Medications, Doses                     │
│  ├─ Real-time sync (multi-user)                             │
│  └─ Offline persistence                                     │
├─────────────────────────────────────────────────────────────┤
│  Storage (Cloud Storage)                                     │
│  ├─ Medication photos (compressed)                          │
│  └─ PDF exports                                             │
├─────────────────────────────────────────────────────────────┤
│  Cloud Functions (Serverless)                                │
│  ├─ Notification Scheduler (runs every 5 minutes)           │
│  ├─ Stripe Webhook Handler                                  │
│  └─ Email Sender (invitations, reports)                     │
├─────────────────────────────────────────────────────────────┤
│  Push Notifications (FCM)                                    │
│  └─ Firebase Cloud Messaging                                │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                   THIRD-PARTY SERVICES                       │
├─────────────────────────────────────────────────────────────┤
│  Stripe (Payments)                                           │
│  Twilio (Optional SMS backup)                                │
│  Sentry (Error tracking)                                     │
│  Mixpanel (Analytics)                                        │
└─────────────────────────────────────────────────────────────┘
```

---

### 1.2 Technology Stack Justification

| Component | Technology | Why This Choice |
|-----------|-----------|-----------------|
| **Mobile Framework** | React Native | Cross-platform (iOS+Android), large community, fast development |
| **Language** | TypeScript | Type safety reduces bugs (critical for health app) |
| **Backend** | Firebase | Serverless = $0 infrastructure costs, built-in features (auth, DB, storage) |
| **Database** | Cloud Firestore | Real-time sync (family coordination), offline support, NoSQL flexibility |
| **Notifications** | Firebase Cloud Messaging | Free, reliable, built into Firebase ecosystem |
| **Payments** | Stripe | Industry standard, 2.9% + $0.30 per transaction, easy integration |
| **State Management** | React Context + Hooks | Simple, no extra dependencies (Redux overkill for MVP) |
| **Navigation** | React Navigation | Standard for React Native, well-documented |
| **Image Handling** | react-native-image-picker + Firebase Storage | Native camera access, cloud storage with CDN |
| **PDF Generation** | react-native-html-to-pdf | Offline PDF creation, customizable templates |
| **Error Tracking** | Sentry | Free tier (5K events/month), React Native support |
| **Analytics** | Mixpanel | Free tier (100K users), event-based tracking |

---

## 2. NOTIFICATION SYSTEM (CRITICAL)

### 2.1 Why Notifications Are Mission-Critical

**The App Lives or Dies on Notification Reliability**

- If notifications fail, medications are missed → dangerous health outcomes
- User trust is destroyed instantly ("Can't rely on this app")
- 1-star reviews: "Reminder didn't go off, Dad missed his insulin"
- Churn rate spikes to 50%+ within days

**Target Reliability: 99.9%** (no more than 1 failure per 1,000 notifications)

---

### 2.2 Notification Architecture

#### **Strategy: Hybrid Approach (Local + Cloud)**

```
┌──────────────────────────────────────────────────────────┐
│                 USER ADDS MEDICATION                      │
│            "Dad's blood pressure pill, 9 AM daily"       │
└──────────────────────────────────────────────────────────┘
                         ↓
         ┌───────────────────────────────┐
         │  SCHEDULE BOTH SIMULTANEOUSLY │
         └───────────────────────────────┘
                         ↓
        ┌────────────────┴─────────────────┐
        ↓                                  ↓
┌──────────────────┐            ┌──────────────────────┐
│ LOCAL NOTIFICATION│            │ CLOUD FUNCTION       │
│ (Device)          │            │ (Firebase)           │
├──────────────────┤            ├──────────────────────┤
│ • Scheduled via  │            │ • Cron job runs      │
│   Notifee        │            │   every 5 minutes    │
│ • Works offline  │            │ • Checks for due     │
│ • Instant        │            │   medications        │
│ • Battery drain  │            │ • Sends FCM push     │
│   risk (iOS)     │            │ • Requires internet  │
└──────────────────┘            └──────────────────────┘
        │                                  │
        └────────────────┬─────────────────┘
                         ↓
              ┌──────────────────┐
              │ NOTIFICATION FIRES│
              │   @ 9:00 AM       │
              └──────────────────┘
```

**Redundancy Logic:**
1. **Primary:** Local notification (Notifee) fires at 9:00 AM
2. **Backup:** Cloud function checks at 8:55 AM, 9:00 AM, 9:05 AM
3. **If both fail:** Log error, alert dev team, send recovery notification

---

### 2.3 Implementation: Local Notifications (Notifee)

**Library:** `@notifee/react-native` (best for React Native)

**Key Features:**
- Schedule notifications locally (works offline)
- Rich notifications (show pill photo, custom actions)
- Android: Full control
- iOS: Limited background scheduling (63 notifications max)

**Code Example:**

```typescript
import notifee, { TimestampTrigger, TriggerType } from '@notifee/react-native';

async function scheduleLocalNotification(medication: Medication, time: Date) {
  // Create notification channel (Android)
  const channelId = await notifee.createChannel({
    id: 'medication-reminders',
    name: 'Medication Reminders',
    importance: AndroidImportance.HIGH, // Critical: shows as heads-up
    sound: 'default',
  });

  // Schedule notification
  const trigger: TimestampTrigger = {
    type: TriggerType.TIMESTAMP,
    timestamp: time.getTime(),
    alarmManager: {
      allowWhileIdle: true, // Critical: fire even in Doze mode
    },
  };

  await notifee.createTriggerNotification(
    {
      id: medication.id,
      title: `Time to give ${medication.patientName} their medication`,
      body: `${medication.name} - ${medication.dosage}`,
      android: {
        channelId,
        smallIcon: 'ic_pill', // Custom icon
        largeIcon: medication.photoUrl, // Show pill photo!
        color: '#1E88E5',
        importance: AndroidImportance.HIGH,
        pressAction: {
          id: 'mark-as-given',
          launchActivity: 'default',
        },
        actions: [
          {
            title: 'Mark as Given',
            pressAction: { id: 'given' },
          },
          {
            title: 'Snooze 15 min',
            pressAction: { id: 'snooze' },
          },
        ],
      },
      ios: {
        sound: 'default',
        categoryId: 'medication-reminder',
        attachments: [
          {
            url: medication.photoUrl, // Show pill photo
          },
        ],
      },
    },
    trigger
  );
}

// iOS limitation: Max 63 scheduled notifications
// Solution: Schedule next 7 days only, refresh weekly
async function refreshLocalNotifications() {
  const allMedications = await getMedications();
  const upcomingDoses = generateUpcomingDoses(allMedications, 7); // Next 7 days

  // Cancel old notifications
  await notifee.cancelAllNotifications();

  // Schedule new ones (limit to 63)
  const limited = upcomingDoses.slice(0, 60); // Leave buffer for other app notifications
  for (const dose of limited) {
    await scheduleLocalNotification(dose.medication, dose.time);
  }
}
```

**iOS Limitation Workaround:**
- iOS only allows 63 scheduled local notifications
- Solution: Schedule next 7 days, refresh when user opens app
- Fallback: Cloud notifications handle anything beyond 7 days

---

### 2.4 Implementation: Cloud Notifications (Firebase Cloud Functions)

**Why Cloud Backup:**
- Handles cases where local notifications fail
- Unlimited scheduling (no 63 notification limit)
- Works when app is uninstalled then reinstalled

**Cloud Function (runs every 5 minutes):**

```typescript
// functions/src/notificationScheduler.ts
import * as functions from 'firebase-functions';
import * as admin from 'firebase-admin';

export const sendDueNotifications = functions.pubsub
  .schedule('every 5 minutes') // Cron: */5 * * * *
  .onRun(async (context) => {
    const now = new Date();
    const fiveMinutesAgo = new Date(now.getTime() - 5 * 60 * 1000);
    const fiveMinutesLater = new Date(now.getTime() + 5 * 60 * 1000);

    // Query Firestore for doses due in this window
    const dueDoesSnapshot = await admin.firestore()
      .collection('doses')
      .where('scheduledTime', '>=', fiveMinutesAgo)
      .where('scheduledTime', '<=', fiveMinutesLater)
      .where('status', '==', 'pending')
      .get();

    const notifications: admin.messaging.Message[] = [];

    for (const doc of dueDosesSnapshot.docs) {
      const dose = doc.data();

      // Get user's FCM tokens (might have multiple devices)
      const userDoc = await admin.firestore()
        .collection('users')
        .doc(dose.caregiverId)
        .get();

      const tokens = userDoc.data()?.fcmTokens || [];

      for (const token of tokens) {
        notifications.push({
          token,
          notification: {
            title: `Time to give ${dose.patientName} their medication`,
            body: `${dose.medicationName} - ${dose.dosage}`,
            imageUrl: dose.photoUrl, // Show pill photo
          },
          data: {
            doseId: doc.id,
            medicationId: dose.medicationId,
            type: 'medication-reminder',
          },
          android: {
            priority: 'high', // Ensure delivery even in Doze mode
            notification: {
              channelId: 'medication-reminders',
              sound: 'default',
            },
          },
          apns: {
            payload: {
              aps: {
                sound: 'default',
                contentAvailable: true,
              },
            },
          },
        });
      }
    }

    // Send all notifications (batched for efficiency)
    if (notifications.length > 0) {
      await admin.messaging().sendAll(notifications);
      console.log(`Sent ${notifications.length} notifications`);
    }

    return null;
  });
```

**Optimization:**
- Runs every 5 minutes (trade-off: precision vs cost)
- Alternative: Schedule individual Cloud Functions per dose (more expensive)
- Batch send notifications (FCM allows 500/batch)

---

### 2.5 Notification Reliability Best Practices

| Challenge | Solution |
|-----------|----------|
| **iOS kills background tasks** | Use `alarmManager.allowWhileIdle` (Android), `content-available` (iOS) |
| **User disables notifications** | In-app prompt to enable, show value ("Never miss Dad's meds") |
| **Battery optimization kills app** | Guide user to disable battery optimization for MediRemind |
| **Notification not seen** | Snooze option, escalating alerts (2nd reminder 15 min later) |
| **Cloud function cold start delays** | Keep function warm with pinger (costs $1/month) |
| **FCM token expires** | Refresh token on app launch, store multiple tokens per user |
| **Time zone issues** | Store all times in UTC, convert to user's timezone |
| **User changes medication schedule** | Cancel old notifications, reschedule immediately |

---

### 2.6 Monitoring Notification Delivery

**Critical Metric: Notification Success Rate**

```typescript
// Track every notification attempt
function trackNotificationSent(doseId: string) {
  analytics.track('notification_sent', {
    doseId,
    timestamp: new Date().toISOString(),
    method: 'cloud', // or 'local'
  });
}

function trackNotificationReceived(doseId: string) {
  analytics.track('notification_received', {
    doseId,
    timestamp: new Date().toISOString(),
  });
}

// Dashboard query:
// Success Rate = (notifications_received / notifications_sent) * 100
// Alert if < 98%
```

**Alerting:**
- Sentry: Alert if notification failure rate > 2%
- PagerDuty: Page on-call dev if critical failure
- User notification: "We noticed you might have missed a reminder. Check notification settings?"

---

## 3. DATABASE SCHEMA

### 3.1 Firestore Collections

**Key Principles:**
- Denormalize for read performance (caregiver dashboard needs to be instant)
- Keep writes idempotent (duplicate "mark as given" shouldn't create errors)
- Use subcollections for scalability (doses within medications)

---

#### **Collection: `users`**

```typescript
interface User {
  id: string; // Firebase Auth UID
  email: string;
  displayName: string;
  createdAt: Timestamp;
  subscription: {
    plan: 'free' | 'premium';
    status: 'active' | 'canceled' | 'past_due';
    stripeCustomerId: string;
    subscriptionId: string;
    currentPeriodEnd: Timestamp;
  };
  fcmTokens: string[]; // Multiple devices
  settings: {
    notificationTime: string; // "09:00" preferred time for daily summary
    timeZone: string; // "America/New_York"
    enableSMS: boolean;
    phoneNumber?: string;
  };
}
```

**Indexes:**
- `subscription.plan` (query all premium users)
- `subscription.currentPeriodEnd` (find expiring subscriptions)

---

#### **Collection: `families`**

```typescript
interface Family {
  id: string;
  name: string; // "Johnson Family"
  createdBy: string; // User ID
  members: {
    userId: string;
    role: 'admin' | 'caregiver' | 'viewer';
    addedAt: Timestamp;
  }[];
  createdAt: Timestamp;
}
```

**Purpose:** Enable multi-user coordination (family members share access to patients)

---

#### **Collection: `patients`**

```typescript
interface Patient {
  id: string;
  familyId: string; // Which family this patient belongs to
  name: string; // "Dad" or "Robert Smith"
  dateOfBirth?: Date;
  photoUrl?: string; // Optional profile photo
  notes?: string; // "Diabetic, allergic to penicillin"
  createdAt: Timestamp;
  createdBy: string; // User ID
}
```

**Indexes:**
- `familyId` (get all patients for a family)

---

#### **Collection: `medications`**

```typescript
interface Medication {
  id: string;
  patientId: string;
  familyId: string; // Denormalized for easier queries
  name: string; // "Lisinopril"
  dosage: string; // "10 mg"
  instructions?: string; // "Take with food"
  photoUrl?: string; // Photo of pill (critical feature!)
  schedule: {
    frequency: 'daily' | 'weekly' | 'asNeeded' | 'custom';
    times: string[]; // ["09:00", "21:00"] in patient's timezone
    daysOfWeek?: number[]; // [0, 2, 4] = Sun, Tue, Thu (for weekly)
    customSchedule?: {
      startDate: Date;
      endDate?: Date;
      interval: number; // e.g., every 8 hours
    };
  };
  refill: {
    enabled: boolean;
    pillsRemaining?: number;
    pillsPerDose: number;
    refillThreshold: number; // Alert when <= this many pills
    lastRefillDate?: Timestamp;
  };
  isActive: boolean; // Soft delete (for history)
  createdAt: Timestamp;
  createdBy: string;
}
```

**Indexes:**
- `patientId` (get all meds for a patient)
- `familyId` + `isActive` (get active meds for family dashboard)

---

#### **Subcollection: `medications/{id}/doses`**

```typescript
interface Dose {
  id: string; // Auto-generated
  medicationId: string; // Parent medication
  patientId: string; // Denormalized
  familyId: string; // Denormalized
  scheduledTime: Timestamp; // When medication should be taken
  status: 'pending' | 'given' | 'missed' | 'skipped';
  completedAt?: Timestamp;
  completedBy?: string; // User ID who marked as given
  notes?: string; // "Taken with breakfast"
  createdAt: Timestamp;
}
```

**Why Subcollection:**
- Doses are conceptually "part of" a medication
- Limits: 1M doses per medication (plenty for lifetime)
- Performance: Queries scoped to specific medication are fast

**Indexes:**
- `scheduledTime` + `status` (find pending doses in time range)
- `patientId` + `scheduledTime` (patient adherence history)

---

#### **Collection: `invitations`**

```typescript
interface Invitation {
  id: string;
  familyId: string;
  invitedEmail: string;
  invitedBy: string; // User ID
  role: 'caregiver' | 'viewer';
  status: 'pending' | 'accepted' | 'expired';
  createdAt: Timestamp;
  expiresAt: Timestamp; // 7 days from creation
}
```

**Purpose:** Invite family members to collaborate

---

### 3.2 Data Access Patterns (Queries)

**Dashboard: Show today's medications for all patients in family**

```typescript
const today = new Date();
today.setHours(0, 0, 0, 0);
const tomorrow = new Date(today);
tomorrow.setDate(tomorrow.getDate() + 1);

const dosesQuery = firestore()
  .collectionGroup('doses') // Query across all medications
  .where('familyId', '==', currentUser.familyId)
  .where('scheduledTime', '>=', today)
  .where('scheduledTime', '<', tomorrow)
  .orderBy('scheduledTime', 'asc');

const snapshot = await dosesQuery.get();
const doses = snapshot.docs.map(doc => doc.data());
```

**Medication History: Adherence for last 30 days**

```typescript
const thirtyDaysAgo = new Date();
thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);

const historyQuery = firestore()
  .collection(`medications/${medicationId}/doses`)
  .where('scheduledTime', '>=', thirtyDaysAgo)
  .orderBy('scheduledTime', 'desc');

const snapshot = await historyQuery.get();
const doses = snapshot.docs.map(doc => doc.data());

// Calculate adherence
const totalDoses = doses.length;
const givenDoses = doses.filter(d => d.status === 'given').length;
const adherenceRate = (givenDoses / totalDoses) * 100;
```

---

### 3.3 Real-Time Sync (Multi-User Coordination)

**Scenario:** Sister marks dose as given, brother sees update instantly

```typescript
// Sister's device: Mark as given
await firestore()
  .collection(`medications/${medicationId}/doses`)
  .doc(doseId)
  .update({
    status: 'given',
    completedAt: new Date(),
    completedBy: currentUser.uid,
  });

// Brother's device: Listen for changes
const unsubscribe = firestore()
  .collection(`medications/${medicationId}/doses`)
  .doc(doseId)
  .onSnapshot((snapshot) => {
    const dose = snapshot.data();
    if (dose.status === 'given') {
      showToast(`${dose.completedByName} marked this medication as given`);
      updateUI(dose);
    }
  });

// Cleanup
unsubscribe(); // When component unmounts
```

**Cost Optimization:**
- Only listen to today's doses (not entire history)
- Unsubscribe when app is backgrounded

---

## 4. AUTHENTICATION & MULTI-USER

### 4.1 Authentication Flow

```
User Opens App
     ↓
┌────────────────┐
│ Is logged in?  │
└────────────────┘
     ↓
    No → Sign Up / Login
     │
     ↓
┌────────────────────────────┐
│ Email/Password or OAuth    │
│ (Google, Apple Sign-In)    │
└────────────────────────────┘
     ↓
┌────────────────────────────┐
│ Create user in Firestore   │
│ - Basic profile            │
│ - Create default family    │
└────────────────────────────┘
     ↓
    Yes → Main App
```

**Implementation:**

```typescript
// @react-native-firebase/auth
import auth from '@react-native-firebase/auth';
import firestore from '@react-native-firebase/firestore';

async function signUpWithEmail(email: string, password: string, name: string) {
  // 1. Create Firebase Auth user
  const userCredential = await auth().createUserWithEmailAndPassword(email, password);
  const uid = userCredential.user.uid;

  // 2. Update profile
  await userCredential.user.updateProfile({ displayName: name });

  // 3. Create Firestore user document
  await firestore().collection('users').doc(uid).set({
    email,
    displayName: name,
    createdAt: firestore.FieldValue.serverTimestamp(),
    subscription: {
      plan: 'free',
      status: 'active',
    },
    fcmTokens: [],
    settings: {
      timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
    },
  });

  // 4. Create default family
  const familyRef = await firestore().collection('families').add({
    name: `${name}'s Family`,
    createdBy: uid,
    members: [
      {
        userId: uid,
        role: 'admin',
        addedAt: firestore.FieldValue.serverTimestamp(),
      },
    ],
    createdAt: firestore.FieldValue.serverTimestamp(),
  });

  // 5. Link family to user
  await firestore().collection('users').doc(uid).update({
    familyId: familyRef.id,
  });

  return userCredential.user;
}
```

---

### 4.2 Family Invitation Flow

```
User A: "Invite sister to help with Dad's meds"
     ↓
┌──────────────────────────┐
│ Enter sister's email     │
│ sarah@example.com        │
└──────────────────────────┘
     ↓
┌──────────────────────────┐
│ Create invitation record │
│ Send email with link     │
└──────────────────────────┘
     ↓
Sister receives email
     ↓
┌──────────────────────────┐
│ Clicks invitation link   │
│ (deep link to app)       │
└──────────────────────────┘
     ↓
┌──────────────────────────┐
│ If new user: Sign up     │
│ If existing: Log in      │
└──────────────────────────┘
     ↓
┌──────────────────────────┐
│ Accept invitation        │
│ - Add to family members  │
│ - Grant access to patients│
└──────────────────────────┘
     ↓
Both users now share access!
```

**Implementation:**

```typescript
// Cloud Function: Send invitation
export const sendInvitation = functions.https.onCall(async (data, context) => {
  const { email, familyId, role } = data;
  const invitedBy = context.auth.uid;

  // Create invitation
  const invitationRef = await admin.firestore().collection('invitations').add({
    familyId,
    invitedEmail: email,
    invitedBy,
    role,
    status: 'pending',
    createdAt: admin.firestore.FieldValue.serverTimestamp(),
    expiresAt: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000), // 7 days
  });

  // Send email (using SendGrid or Firebase Extensions)
  const invitationLink = `https://mediremind.app/invite/${invitationRef.id}`;
  await sendEmail({
    to: email,
    subject: 'You've been invited to collaborate on MediRemind',
    html: `
      <p>You've been invited to help manage medications for your family.</p>
      <a href="${invitationLink}">Accept Invitation</a>
    `,
  });

  return { success: true, invitationId: invitationRef.id };
});

// App: Accept invitation
async function acceptInvitation(invitationId: string) {
  const invitationDoc = await firestore().collection('invitations').doc(invitationId).get();
  const invitation = invitationDoc.data();

  if (invitation.status !== 'pending') {
    throw new Error('Invitation already used or expired');
  }

  // Add user to family
  await firestore().collection('families').doc(invitation.familyId).update({
    members: firestore.FieldValue.arrayUnion({
      userId: currentUser.uid,
      role: invitation.role,
      addedAt: new Date(),
    }),
  });

  // Update user's familyId
  await firestore().collection('users').doc(currentUser.uid).update({
    familyId: invitation.familyId,
  });

  // Mark invitation as accepted
  await invitationDoc.ref.update({ status: 'accepted' });
}
```

---

## 5. PHOTO STORAGE & OPTIMIZATION

### 5.1 Image Upload Flow

```
User adds medication → Tap "Add Photo" → Opens camera/gallery
     ↓
Capture/select image (3000x2000, 2.5MB JPEG)
     ↓
┌────────────────────────────┐
│ RESIZE & COMPRESS (on device)│
│ - Max dimensions: 800x800   │
│ - Quality: 70%              │
│ - Output: ~50KB JPEG        │
└────────────────────────────┘
     ↓
┌────────────────────────────┐
│ Upload to Firebase Storage │
│ Path: /medications/{id}.jpg│
└────────────────────────────┘
     ↓
Get download URL
     ↓
Save URL to Firestore medication document
```

**Why Resize:**
- Original: 2.5MB × 1,000 users × 5 meds = 12.5GB storage ($0.026/GB = $325/month)
- Compressed: 50KB × 1,000 users × 5 meds = 250MB ($0.026/GB = $6.50/month)
- **Savings: 98% reduction in storage costs**

---

### 5.2 Implementation

```typescript
import { launchCamera, launchImageLibrary } from 'react-native-image-picker';
import storage from '@react-native-firebase/storage';
import ImageResizer from 'react-native-image-resizer';

async function uploadMedicationPhoto(medicationId: string): Promise<string> {
  // 1. Launch camera/gallery
  const result = await launchCamera({
    mediaType: 'photo',
    cameraType: 'back',
    quality: 0.8,
  });

  if (result.didCancel || !result.assets[0]) {
    return null;
  }

  const originalUri = result.assets[0].uri;

  // 2. Resize & compress
  const resized = await ImageResizer.createResizedImage(
    originalUri,
    800, // maxWidth
    800, // maxHeight
    'JPEG',
    70, // quality (0-100)
    0, // rotation
  );

  // 3. Upload to Firebase Storage
  const filename = `${medicationId}.jpg`;
  const reference = storage().ref(`medications/${filename}`);

  await reference.putFile(resized.uri);

  // 4. Get download URL
  const downloadUrl = await reference.getDownloadURL();

  // 5. Update Firestore
  await firestore().collection('medications').doc(medicationId).update({
    photoUrl: downloadUrl,
  });

  return downloadUrl;
}
```

---

### 5.3 Image Caching (Performance)

**Problem:** Re-downloading images wastes bandwidth and slows app

**Solution:** Use `react-native-fast-image` (caches images automatically)

```typescript
import FastImage from 'react-native-fast-image';

function MedicationCard({ medication }) {
  return (
    <FastImage
      source={{
        uri: medication.photoUrl,
        priority: FastImage.priority.high,
        cache: FastImage.cacheControl.immutable, // Never refetch
      }}
      style={{ width: 100, height: 100, borderRadius: 8 }}
    />
  );
}
```

---

## 6. OFFLINE-FIRST ARCHITECTURE

### 6.1 Why Offline Support Matters

**User Scenarios:**
- Caregiver in rural area with spotty internet
- Hospital basement (no signal)
- Airplane mode during travel

**Requirements:**
- Core features work offline: view schedule, mark doses as given
- Sync changes when back online
- Conflict resolution (rare: two caregivers mark same dose)

---

### 6.2 Offline Implementation

**Firestore Offline Persistence (Built-In):**

```typescript
import firestore from '@react-native-firebase/firestore';

// Enable offline persistence (default in React Native Firebase)
// Data is cached locally in SQLite
firestore().settings({
  persistence: true,
  cacheSizeBytes: firestore.CACHE_SIZE_UNLIMITED,
});

// Queries work offline automatically
const doses = await firestore()
  .collection('doses')
  .where('familyId', '==', familyId)
  .get({ source: 'cache' }); // Force cache read
```

**How It Works:**
1. First time online: Data downloaded, stored in local SQLite
2. Offline: Reads from cache, writes queued
3. Back online: Queued writes synced, cache updated

---

### 6.3 Conflict Resolution

**Scenario:** Sister marks dose as given offline, brother does same → both come online

**Firestore Strategy:**
- Last write wins (default)
- Use `serverTimestamp()` for authoritative ordering

**Better Strategy: Idempotent Writes**

```typescript
// Instead of simple update
await doseRef.update({ status: 'given' });

// Use transaction (atomic)
await firestore().runTransaction(async (transaction) => {
  const doseDoc = await transaction.get(doseRef);
  const dose = doseDoc.data();

  if (dose.status === 'pending') {
    transaction.update(doseRef, {
      status: 'given',
      completedAt: firestore.FieldValue.serverTimestamp(),
      completedBy: currentUser.uid,
    });
  } else {
    // Already marked by someone else
    console.log('Dose already completed by another user');
  }
});
```

---

## 7. SECURITY & PRIVACY

### 7.1 Firestore Security Rules

```javascript
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {

    // Users can only read/write their own user document
    match /users/{userId} {
      allow read, write: if request.auth.uid == userId;
    }

    // Family members can read/write family data
    match /families/{familyId} {
      allow read: if isUserInFamily(familyId);
      allow write: if isUserInFamily(familyId) && hasRole(familyId, 'admin');
    }

    // Patients: only family members can access
    match /patients/{patientId} {
      allow read, write: if isUserInFamily(resource.data.familyId);
    }

    // Medications: only family members
    match /medications/{medicationId} {
      allow read, write: if isUserInFamily(resource.data.familyId);

      // Doses subcollection
      match /doses/{doseId} {
        allow read, write: if isUserInFamily(resource.data.familyId);
      }
    }

    // Helper functions
    function isUserInFamily(familyId) {
      return request.auth.uid != null &&
             exists(/databases/$(database)/documents/families/$(familyId)) &&
             request.auth.uid in get(/databases/$(database)/documents/families/$(familyId)).data.members;
    }

    function hasRole(familyId, role) {
      let family = get(/databases/$(database)/documents/families/$(familyId)).data;
      let member = family.members[request.auth.uid];
      return member != null && member.role == role;
    }
  }
}
```

---

### 7.2 Storage Security Rules

```javascript
rules_version = '2';
service firebase.storage {
  match /b/{bucket}/o {
    match /medications/{medicationId}.jpg {
      // Only authenticated users in the same family can read
      allow read: if request.auth != null;

      // Only authenticated users can write their own medication photos
      allow write: if request.auth != null &&
                      request.resource.size < 1 * 1024 * 1024 && // Max 1MB
                      request.resource.contentType.matches('image/.*');
    }
  }
}
```

---

### 7.3 Data Privacy Best Practices

| Principle | Implementation |
|-----------|----------------|
| **Minimize data collection** | Don't ask for patient SSN, insurance, etc. (not needed) |
| **Encrypt in transit** | HTTPS (Firebase default) |
| **Encrypt at rest** | Firebase encrypts data at rest (automatic) |
| **No PHI integration** | Don't connect to EHRs (avoids HIPAA) |
| **User data export** | Allow users to download their data (GDPR compliance) |
| **User data deletion** | Allow users to delete account + all data |
| **Audit logging** | Log who accessed what (for family transparency) |

**GDPR Compliance (if serving EU users):**
- Privacy policy (required)
- Cookie consent (if using web)
- Data portability (export feature)
- Right to deletion (account deletion feature)

---

## 8. TESTING STRATEGY

### 8.1 Test Pyramid

```
              ┌────────────────┐
              │   Manual QA    │  ← 5% (critical user flows)
              └────────────────┘
         ┌──────────────────────────┐
         │  Integration Tests       │  ← 20% (API, notifications)
         └──────────────────────────┘
    ┌─────────────────────────────────────┐
    │        Unit Tests                   │  ← 75% (business logic)
    └─────────────────────────────────────┘
```

---

### 8.2 Unit Tests (Jest)

**Test Business Logic:**

```typescript
// medicationScheduler.test.ts
import { generateUpcomingDoses } from './medicationScheduler';

describe('Medication Scheduler', () => {
  it('generates daily doses correctly', () => {
    const medication = {
      id: '123',
      name: 'Lisinopril',
      schedule: {
        frequency: 'daily',
        times: ['09:00', '21:00'],
      },
    };

    const doses = generateUpcomingDoses(medication, 7); // Next 7 days

    expect(doses).toHaveLength(14); // 7 days × 2 times/day
    expect(doses[0].scheduledTime.getHours()).toBe(9);
    expect(doses[1].scheduledTime.getHours()).toBe(21);
  });

  it('handles weekly schedule', () => {
    const medication = {
      id: '456',
      name: 'Weekly Vitamin',
      schedule: {
        frequency: 'weekly',
        times: ['09:00'],
        daysOfWeek: [0, 3], // Sunday, Wednesday
      },
    };

    const doses = generateUpcomingDoses(medication, 14); // 2 weeks

    expect(doses).toHaveLength(4); // 2 weeks × 2 days/week
  });
});
```

**Target Coverage: 80%+ for core business logic**

---

### 8.3 Integration Tests (Detox)

**Test User Flows:**

```typescript
// e2e/addMedication.test.js
describe('Add Medication Flow', () => {
  beforeAll(async () => {
    await device.launchApp();
  });

  it('should add a new medication successfully', async () => {
    // Login
    await element(by.id('email-input')).typeText('test@example.com');
    await element(by.id('password-input')).typeText('password123');
    await element(by.id('login-button')).tap();

    // Navigate to Add Medication
    await element(by.id('add-medication-button')).tap();

    // Fill form
    await element(by.id('medication-name')).typeText('Lisinopril');
    await element(by.id('dosage')).typeText('10 mg');
    await element(by.id('time-picker')).tap();
    // ... set time

    // Save
    await element(by.id('save-button')).tap();

    // Verify medication appears in list
    await expect(element(by.text('Lisinopril'))).toBeVisible();
  });
});
```

---

### 8.4 Notification Testing (Critical)

**Manual Test Checklist:**

| Scenario | Expected Behavior | Status |
|----------|-------------------|--------|
| App in foreground | Notification appears as banner | ✅ |
| App in background | Notification appears in tray | ✅ |
| App force-closed | Notification still fires | ✅ |
| Device in Doze mode (Android) | Notification fires (allowWhileIdle) | ✅ |
| Airplane mode | Local notification fires, cloud delayed | ✅ |
| Time zone change | Notification adjusts to new time zone | ✅ |
| User taps notification | Opens app to medication detail | ✅ |
| User taps "Mark as Given" | Dose marked, notification dismissed | ✅ |
| User taps "Snooze" | Notification reappears in 15 min | ✅ |

**Automated Testing:**
- Use Firebase Test Lab (physical devices in cloud)
- Schedule test notification, verify receipt within 60 seconds

---

## 9. DEPLOYMENT & CI/CD

### 9.1 Environments

| Environment | Purpose | Backend | App Store |
|-------------|---------|---------|-----------|
| **Development** | Local testing | Firebase Dev Project | N/A |
| **Staging** | QA testing | Firebase Staging Project | TestFlight (iOS), Internal Testing (Android) |
| **Production** | Live users | Firebase Prod Project | App Store, Google Play |

---

### 9.2 CI/CD Pipeline (GitHub Actions)

```yaml
# .github/workflows/build.yml
name: Build & Test

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: 18
      - run: npm install
      - run: npm test # Unit tests
      - run: npm run lint

  build-ios:
    runs-on: macos-latest
    needs: test
    steps:
      - uses: actions/checkout@v3
      - run: npm install
      - run: cd ios && pod install
      - run: npx react-native build-ios --mode Release
      - run: npx fastlane ios beta # Upload to TestFlight

  build-android:
    runs-on: ubuntu-latest
    needs: test
    steps:
      - uses: actions/checkout@v3
      - run: npm install
      - run: cd android && ./gradlew assembleRelease
      - run: npx fastlane android beta # Upload to Play Console
```

---

## 10. COST OPTIMIZATION

### 10.1 Firebase Cost Breakdown (Monthly)

**Months 1-6 (0-500 users): ~$25/month**

| Service | Usage | Cost |
|---------|-------|------|
| Firestore | 50K reads/day, 10K writes/day | $0 (free tier) |
| Storage | 1GB (500 users × 5 meds × 50KB) | $0.026 |
| Cloud Functions | 100K invocations/day | $0 (free tier) |
| FCM | 10K notifications/day | $0 (free) |
| Hosting | 10GB/month | $0 (free tier) |
| **Total** | | **~$0-25** |

**Months 7-12 (500-2000 users): ~$150/month**

| Service | Usage | Cost |
|---------|-------|------|
| Firestore | 200K reads/day, 50K writes/day | $60 |
| Storage | 5GB | $0.13 |
| Cloud Functions | 500K invocations/day | $40 |
| FCM | 50K notifications/day | $0 |
| Hosting | 50GB/month | $10 |
| **Total** | | **~$150** |

---

### 10.2 Cost Optimization Tactics

**1. Reduce Firestore Reads**
```typescript
// BAD: Reads entire collection every time
const medications = await firestore().collection('medications').get();

// GOOD: Cache locally, only fetch changes
const lastFetch = await AsyncStorage.getItem('lastMedicationFetch');
const query = firestore()
  .collection('medications')
  .where('updatedAt', '>', lastFetch);
```

**2. Batch Writes**
```typescript
// BAD: 10 medications = 10 writes
for (const med of medications) {
  await firestore().collection('medications').add(med);
}

// GOOD: 10 medications = 1 batch write
const batch = firestore().batch();
medications.forEach(med => {
  const ref = firestore().collection('medications').doc();
  batch.set(ref, med);
});
await batch.commit();
```

**3. Compress Images**
- Already covered in Section 5

**4. Use Cloud Function Cold Start Optimization**
```typescript
// Keep function warm with scheduled ping
export const keepWarm = functions.pubsub
  .schedule('every 5 minutes')
  .onRun(() => {
    console.log('Keeping warm');
    return null;
  });
```

---

## 11. MONITORING & ERROR TRACKING

### 11.1 Sentry Setup

```typescript
import * as Sentry from '@sentry/react-native';

Sentry.init({
  dsn: 'https://your-dsn@sentry.io/project-id',
  environment: __DEV__ ? 'development' : 'production',
  tracesSampleRate: 0.2, // 20% of transactions
});

// Catch errors
try {
  await firestore().collection('medications').add(medication);
} catch (error) {
  Sentry.captureException(error, {
    contexts: {
      medication: {
        id: medication.id,
        name: medication.name,
      },
    },
  });
  throw error;
}
```

---

### 11.2 Key Metrics Dashboard (Mixpanel)

**Events to Track:**

| Event | Properties | Why |
|-------|-----------|-----|
| `user_signed_up` | `source` (organic, ad) | Attribution |
| `medication_added` | `patient_id`, `has_photo` | Activation |
| `notification_received` | `dose_id`, `on_time` | Reliability |
| `dose_marked_given` | `dose_id`, `time_delta` | Engagement |
| `subscription_started` | `plan`, `price` | Revenue |
| `app_opened` | `session_length` | Retention |

**Dashboard Queries:**
- Daily Active Users (DAU)
- Notification delivery rate
- Free → Paid conversion funnel
- Churn rate by cohort

---

## 12. SCALABILITY CONSIDERATIONS

### 12.1 Current Architecture Limits

| Component | Current Limit | When to Migrate |
|-----------|---------------|-----------------|
| Firestore | 1M concurrent connections | 100K users |
| Cloud Functions | 1000 concurrent executions | 50K users |
| Firebase Storage | 50TB | 10M photos (unlikely) |
| FCM | 500 messages/second | 100K users |

**Realistic Growth:**
- Year 1: 2,000 users (well within limits)
- Year 2: 10,000 users (still safe)
- Year 3: 50,000 users (monitor, optimize)
- Year 4+: 100K+ users (consider microservices migration)

---

### 12.2 Migration Path (If Needed)

**Option 1: Optimize Firebase**
- Enable caching, reduce queries
- Use Cloud Firestore data bundles
- Defer: Stay on Firebase until $500/month costs

**Option 2: Hybrid (Keep Firebase Auth, Migrate Database)**
- Move to Supabase (PostgreSQL) for complex queries
- Keep Firebase for auth, storage, FCM
- Cost: ~$200/month for 50K users (vs $1,500 on Firebase)

**Option 3: Full Custom Backend (Last Resort)**
- AWS: RDS (PostgreSQL), Lambda, S3, SNS
- Complexity: 10x higher
- Cost: ~$300/month for 50K users
- Only if: Revenue > $50K/month

---

## CONCLUSION

### Critical Success Factors

1. **Notification Reliability (99.9%)**
   - Hybrid local + cloud approach
   - Extensive testing on real devices
   - Monitoring and alerting

2. **Simple UX**
   - Core flow: Add med → Get notification → Mark as given (< 30 seconds)
   - Avoid feature bloat in MVP

3. **Cost Discipline**
   - Stay on Firebase free tier as long as possible
   - Optimize images, queries, function invocations
   - Target: <$200/month until 2,000 users

4. **Security & Privacy**
   - Firestore security rules (tested!)
   - No unnecessary data collection
   - Clear privacy policy

### Next Steps

**Week 1:**
1. Set up Firebase project (Dev, Staging, Prod)
2. Initialize React Native app (`npx react-native init MediRemind --template typescript`)
3. Install dependencies (`@react-native-firebase/app`, etc.)

**Week 2-4:**
4. Build core screens (Login, Dashboard, Add Medication)
5. Implement notification system (local + cloud)
6. Test notifications extensively (iOS, Android, various scenarios)

**Week 5-8:**
7. Multi-user features (family sharing)
8. Photo upload & compression
9. Beta testing with 50 users

**Week 9-12:**
10. Polish UX based on feedback
11. App store submission
12. Launch! 🚀

---

**Document Version:** 1.0
**Technology Stack:** React Native 0.79 + Firebase + TypeScript
**Estimated Development Time:** 12 weeks (1 developer, full-time)
**Target Budget:** <$5,000 Year 1 infrastructure costs
