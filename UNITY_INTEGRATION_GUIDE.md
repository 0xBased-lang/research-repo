# Merge Bounce - Unity Integration Guide

## Complete Setup Instructions

---

## 📋 Prerequisites

**Required Software:**
- Unity Hub (latest version)
- Unity 2022.3 LTS or newer
- Visual Studio Code or Visual Studio 2022
- Git (for version control)

**Required Accounts:**
- Firebase account (free at firebase.google.com)
- Apple Developer ($99/year - for iOS only)
- Google Play Developer ($25 one-time - for Android only)

---

## 🚀 Part 1: Firebase Setup (15 minutes)

### Step 1: Create Firebase Project

1. Go to https://console.firebase.google.com
2. Click **"Add Project"**
3. Project name: `merge-bounce-game`
4. Enable Google Analytics: **Yes**
5. Choose Analytics account or create new
6. Click **"Create Project"**
7. Wait for project creation (30-60 seconds)

### Step 2: Enable Authentication

1. In Firebase console, click **"Authentication"** in left menu
2. Click **"Get Started"**
3. Click **"Sign-in method"** tab
4. Enable **"Email/Password"**
5. Click **"Save"**

Optional: Enable Google, Apple, Facebook sign-in here

### Step 3: Create Firestore Database

1. Click **"Firestore Database"** in left menu
2. Click **"Create database"**
3. Choose **"Production mode"**
4. Select location: **us-central** (or nearest to you)
5. Click **"Enable"**

### Step 4: Deploy Backend Functions

```bash
# Navigate to backend folder
cd merge-bounce-backend

# Install Firebase CLI globally
npm install -g firebase-tools

# Login to Firebase
firebase login

# Initialize Firebase in project
firebase init

# Select:
# - Functions (use existing code)
# - Firestore (use firestore.rules)
# Choose your project: merge-bounce-game
# Language: JavaScript
# ESLint: No
# Install dependencies: Yes

# Deploy everything
firebase deploy

# You'll see:
# ✔  Deploy complete!
# Functions deployed: createUserProfile, saveProgress, submitScore, etc.
```

**Result:** Backend is live! ✅

### Step 5: Get Configuration Files

#### For iOS:
1. In Firebase console, click ⚙️ **Settings** > **Project Settings**
2. Scroll to **"Your apps"**
3. Click **iOS** icon
4. iOS bundle ID: `com.yourstudio.mergebounce`
5. App nickname: `Merge Bounce iOS`
6. Click **"Register app"**
7. **Download GoogleService-Info.plist**
8. Save this file - you'll need it for Unity

#### For Android:
1. Same location in Firebase console
2. Click **Android** icon
3. Android package name: `com.yourstudio.mergebounce`
4. App nickname: `Merge Bounce Android`
5. Click **"Register app"**
6. **Download google-services.json**
7. Save this file - you'll need it for Unity

---

## 🎮 Part 2: Unity Project Setup (30 minutes)

### Step 1: Create Unity Project

1. Open **Unity Hub**
2. Click **"New Project"**
3. Template: **2D Core**
4. Project name: `MergeBounce`
5. Location: Choose your workspace
6. Click **"Create Project"**

### Step 2: Import Firebase Unity SDK

1. Download Firebase Unity SDK:
   - Go to: https://firebase.google.com/download/unity
   - Download: `firebase_unity_sdk_11.x.x.zip`

2. Extract the ZIP file

3. In Unity:
   - Assets > Import Package > Custom Package
   - Navigate to extracted folder
   - Import these packages:
     - `FirebaseAuth.unitypackage`
     - `FirebaseFunctions.unitypackage`
     - `FirebaseFirestore.unitypackage`
     - `FirebaseAnalytics.unitypackage` (optional)

4. Wait for import (2-3 minutes)

### Step 3: Add Configuration Files

1. Copy `google-services.json` to:
   - `Assets/Plugins/Android/google-services.json`

2. Copy `GoogleService-Info.plist` to:
   - `Assets/GoogleService-Info.plist`

3. Unity will automatically detect these files

### Step 4: Import Game Scripts

1. In Unity, create folder structure:
   ```
   Assets/
   ├── Scripts/
   │   └── MergeBounce/
   ├── Prefabs/
   ├── Scenes/
   ├── Materials/
   ├── Sprites/
   └── Audio/
   ```

2. Copy all `.cs` files from `merge-bounce-unity/Scripts/` to `Assets/Scripts/MergeBounce/`:
   - GameManager.cs
   - BallController.cs
   - MergeSystem.cs
   - ComboManager.cs
   - ScoreManager.cs
   - PowerUpSystem.cs
   - BackendConnector.cs
   - UIManager.cs
   - AudioManager.cs
   - GameJuice.cs

3. Wait for Unity to compile (30 seconds)

### Step 5: Create Main Scene

1. **Create GameManager GameObject:**
   - Right-click in Hierarchy > Create Empty
   - Name: `GameManager`
   - Add Component > Scripts > GameManager
   - Add Component > Scripts > MergeSystem
   - Add Component > Scripts > ComboManager
   - Add Component > Scripts > ScoreManager
   - Add Component > Scripts > PowerUpSystem
   - Add Component > Scripts > BackendConnector
   - Add Component > Scripts > AudioManager
   - Add Component > Scripts > GameJuice

2. **Create Camera:**
   - Already exists as "Main Camera"
   - Position: (0, 0, -10)
   - Size: 5 (orthographic)

3. **Create Canvas (UI):**
   - Right-click > UI > Canvas
   - Name: `UICanvas`
   - Render Mode: Screen Space - Overlay
   - Add Component > Scripts > UIManager

4. **Create Ball Prefab:**
   - Right-click > 2D Object > Sprite > Circle
   - Name: `Ball`
   - Add Component > Rigidbody 2D
   - Add Component > Circle Collider 2D
   - Add Component > Scripts > BallController
   - Drag to Prefabs folder
   - Delete from scene

### Step 6: Configure Physics

1. Edit > Project Settings > Physics 2D
2. Gravity Y: -9.81 (or adjust for game feel)
3. Create Physics Material 2D:
   - Right-click > Create > Physics Material 2D
   - Name: `BallPhysics`
   - Bounciness: 0.7
   - Friction: 0.3
4. Assign to Ball prefab's Circle Collider

### Step 7: Setup Tags and Layers

1. Edit > Project Settings > Tags and Layers
2. Add Tags:
   - `Ball`
   - `Peg`
   - `Ground`
   - `PowerUp`

3. Assign tags to prefabs

---

## 🔌 Part 3: Backend Integration Testing (10 minutes)

### Test Firebase Connection

1. Create test script:

```csharp
using UnityEngine;
using MergeBounce;

public class TestFirebase : MonoBehaviour
{
    async void Start()
    {
        Debug.Log("Testing Firebase...");

        // Wait for initialization
        await System.Threading.Tasks.Task.Delay(2000);

        if (BackendConnector.Instance.isInitialized)
        {
            Debug.Log("✅ Firebase connected!");

            // Test authentication
            bool signedUp = await BackendConnector.Instance.SignUpWithEmail(
                "test@example.com",
                "password123",
                "TestPlayer"
            );

            if (signedUp)
            {
                Debug.Log("✅ Authentication works!");
            }
        }
        else
        {
            Debug.LogError("❌ Firebase not initialized");
        }
    }
}
```

2. Attach to a GameObject
3. Press Play
4. Check Console for "✅ Firebase connected!"

### Test Cloud Functions

```csharp
// Test leaderboard
var leaderboard = await BackendConnector.Instance.GetLeaderboard("allTime", 10);
Debug.Log($"Leaderboard entries: {leaderboard?.Count}");

// Test save/load
var saveData = new GameSaveData {
    coins = 100,
    gems = 5,
    highScore = 1000
};
await BackendConnector.Instance.SaveProgress(saveData);
Debug.Log("✅ Save successful");

var loadedData = await BackendConnector.Instance.LoadProgress();
Debug.Log($"✅ Loaded: {loadedData.coins} coins");
```

---

## 🎨 Part 4: Visual Setup (60 minutes)

### Create Ball Visuals

1. **Ball Sprite:**
   - Create 2D circle sprite (128x128px)
   - Or use Unity's built-in circle sprite
   - Assign to Ball prefab's Sprite Renderer

2. **Ball Colors (code already handles this):**
   - 2: Red
   - 4: Orange
   - 8: Yellow
   - 16: Green
   - 32: Blue
   - 64: Purple
   - 128: Pink
   - 256: Gold
   - 512: Silver
   - 1024: White
   - 2048: Platinum
   - 4096: Black

3. **Trail Effect:**
   - Ball prefab > Add Component > Trail Renderer
   - Time: 0.5
   - Width: 0.1 to 0.05
   - Color: Gradient matching ball

### Create Particle Effects

1. **Merge Explosion:**
   - Right-click > Effects > Particle System
   - Name: `MergeExplosion`
   - Duration: 0.5
   - Start Lifetime: 0.5
   - Start Speed: 5
   - Start Size: 0.2
   - Start Color: Gradient (bright colors)
   - Emission > Rate: 50
   - Shape > Sphere
   - Save as prefab

2. **Peg Hit Sparks:**
   - Similar to above but smaller
   - Emission > Burst: 10 particles
   - Start Speed: 2
   - Start Size: 0.1

### Create UI

1. **Main Menu:**
   - Canvas > Create > Panel
   - Name: `MainMenuScreen`
   - Add children:
     - Text: "MERGE BOUNCE" (title)
     - Button: "PLAY"
     - Button: "LEADERBOARD"
     - Button: "SHOP"

2. **Game HUD:**
   - Canvas > Create > Panel
   - Name: `GamePlayScreen`
   - Add children:
     - Text: Score (top center)
     - Text: Combo (top left)
     - Image: Combo timer bar
     - Text: Coins (top right)

3. **Game Over:**
   - Canvas > Create > Panel
   - Name: `GameOverScreen`
   - Add children:
     - Text: "GAME OVER"
     - Text: Final score
     - Text: Rank
     - Button: "RESTART"
     - Button: "MENU"

4. **Link to UIManager:**
   - Select UICanvas GameObject
   - UIManager component
   - Drag each screen to corresponding field

---

## 📱 Part 5: Building for Mobile (30 minutes)

### iOS Build

**Requirements:**
- Mac computer
- Xcode installed
- Apple Developer account

**Steps:**
1. File > Build Settings
2. Platform: iOS
3. Click "Switch Platform"
4. Player Settings:
   - Company Name: Your Studio
   - Product Name: Merge Bounce
   - Bundle Identifier: com.yourstudio.mergebounce
   - Version: 1.0.0
5. Click "Build"
6. Choose location
7. Wait for build (5-10 minutes)
8. Open `.xcodeproj` in Xcode
9. Sign with your Apple Developer account
10. Build and Run on device

### Android Build

**Requirements:**
- Android SDK (installed via Unity Hub)
- Android device or emulator

**Steps:**
1. File > Build Settings
2. Platform: Android
3. Click "Switch Platform"
4. Player Settings:
   - Company Name: Your Studio
   - Product Name: Merge Bounce
   - Package Name: com.yourstudio.mergebounce
   - Version: 1.0.0
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Highest installed
5. Click "Build"
6. Choose location
7. Wait for build (3-5 minutes)
8. Install APK on device

---

## 🧪 Part 6: Testing Checklist

### Gameplay Tests:
- ✅ Ball launches when dragged and released
- ✅ Ball bounces off pegs
- ✅ Two same-value balls merge when touching
- ✅ Merged ball has correct value (doubled)
- ✅ Combo counter increases on consecutive merges
- ✅ Combo timer resets on new merge
- ✅ Combo breaks after 3 seconds
- ✅ Score increases on each merge
- ✅ Combo multiplier applies to score
- ✅ Power-up spawns every 5 balls
- ✅ Power-up effects work correctly
- ✅ Extreme Fever triggers at 5x combo
- ✅ Game over when balls reach top

### Backend Tests:
- ✅ Firebase initializes successfully
- ✅ User can sign up
- ✅ User can sign in
- ✅ Save progress to cloud
- ✅ Load progress from cloud
- ✅ Submit score to leaderboard
- ✅ View leaderboard (top 100)
- ✅ Get player's rank
- ✅ Daily challenges load
- ✅ Challenge progress updates

### UI Tests:
- ✅ Main menu displays
- ✅ Play button starts game
- ✅ Game HUD shows correctly
- ✅ Score updates in real-time
- ✅ Combo text appears/disappears
- ✅ Pause menu works
- ✅ Game over screen shows stats
- ✅ Restart button works
- ✅ Leaderboard loads and displays

---

## 🐛 Common Issues & Fixes

### Firebase won't initialize
**Error:** "FirebaseApp failed to initialize"
**Fix:**
- Check `google-services.json` is in `Assets/Plugins/Android/`
- Check `GoogleService-Info.plist` is in `Assets/`
- Reimport Firebase packages
- Restart Unity

### Balls don't merge
**Problem:** Balls touching but not merging
**Fix:**
- Check both balls have same value
- Check BallController script is attached
- Check MergeSystem is on GameManager
- Check tags are set correctly ("Ball")

### Physics feels wrong
**Problem:** Balls bounce too much/too little
**Fix:**
- Adjust Physics Material 2D bounciness
- Adjust Rigidbody2D gravity scale
- Adjust ball launch power in BallController

### UI not showing
**Problem:** UI elements not visible
**Fix:**
- Check Canvas Render Mode (should be Screen Space - Overlay)
- Check UI elements are children of Canvas
- Check UIManager fields are assigned
- Check screens are active in hierarchy

---

## 📊 Performance Optimization

### Reduce Draw Calls:
- Use sprite atlases
- Batch UI elements
- Combine meshes where possible

### Optimize Physics:
- Limit max number of balls on screen (destroy old ones)
- Use appropriate collision layers
- Set Physics2D.autoSyncTransforms = false

### Reduce Memory:
- Compress textures
- Use object pooling for balls
- Unload unused assets

---

## 🚀 Deployment to Stores

### Apple App Store

1. **Prepare:**
   - App icon (1024x1024)
   - Screenshots (all required sizes)
   - Privacy policy URL
   - App description

2. **App Store Connect:**
   - Create app listing
   - Upload build via Xcode
   - Submit for review
   - Wait 1-3 days for approval

### Google Play Store

1. **Prepare:**
   - App icon (512x512)
   - Feature graphic (1024x500)
   - Screenshots (at least 2)
   - Privacy policy URL
   - App description

2. **Play Console:**
   - Create app
   - Upload AAB (Android App Bundle)
   - Fill out store listing
   - Submit for review
   - Wait 1-3 days for approval

---

## 📈 Post-Launch

### Monitor Analytics:
- Check Firebase Analytics dashboard
- Track user retention (Day 1, 7, 30)
- Monitor crash reports
- Check leaderboard activity

### Update Backend:
```bash
# Make changes to cloud functions
cd merge-bounce-backend

# Deploy updates
firebase deploy --only functions

# No Unity rebuild needed!
```

### Update Game:
1. Make changes in Unity
2. Increment version number
3. Build new version
4. Submit update to stores

---

## ✅ You're Done!

Your Merge Bounce game is now:
- ✅ Fully functional
- ✅ Connected to cloud backend
- ✅ Ready for stores
- ✅ Scalable to 1M+ users

**Total Development Time:** ~6 hours
**Cost:** $0-124 (platform fees only)
**Backend Cost:** $0/month for first 5,000 users

**Questions?** Check the API documentation or Firebase docs!
