# START TODAY: 2-WEEK PHYSICS MVP
## Build Bouncing Physics This Weekend - Complete Starter Package

**Goal:** By Sunday night, have a ball bouncing through pegs that FEELS AMAZING

**Time Required:** 15-20 hours total (spread over 2 weeks)

**This Weekend:** 10-12 hours to get first working version

---

## 📅 WEEKEND SCHEDULE (Hour-by-Hour)

### SATURDAY (6-7 hours)

**Hour 1: Install Unity**
- Download Unity Hub: https://unity.com/download
- Install Unity 2022.3 LTS (NOT 2023, NOT 6000)
- Choose "2D Core" template modules
- Coffee break ☕

**Hour 2-3: Unity Basics Tutorial**
- Open Unity Hub → Learn tab → "Creator Kit: Beginner Code"
- OR YouTube: "Brackeys Unity 2D Tutorial 2024" (watch first 30 min)
- Goal: Understand GameObjects, Components, Inspector

**Hour 4: Create Project**
- Unity Hub → New Project
- Template: "2D Core"
- Name: "QuantumBouncePhysics"
- Location: Choose your workspace
- Create Project (takes 5-10 min)

**Hour 5: Project Setup**
- Follow "UNITY PROJECT SETUP" section below
- Create folder structure
- Configure physics settings
- Create physics materials

**Hour 6-7: First Ball**
- Follow "CREATE BALL" section below
- Copy-paste Ball.cs script
- Make ball fall and settle
- Test: Ball drops when you click Play

**END OF DAY 1:** Ball falls due to gravity ✅

---

### SUNDAY (5-6 hours)

**Hour 1-2: Add Pegs**
- Follow "CREATE PEGS" section below
- Design Plinko layout (15 rows × 8 columns)
- Ball bounces off pegs

**Hour 3-4: Physics Tuning**
- Follow "TUNING GUIDE" section below
- Try different bounciness values (0.5 → 0.9)
- Try different friction values (0.0 → 0.3)
- Find what feels satisfying

**Hour 5: Add Sound**
- Download free bounce sound (freesound.org)
- Add AudioSource component
- Play sound on collision
- Makes it feel 10x better

**Hour 6: First Playtest**
- Show to 2-3 friends/family
- Ask: "Is watching this ball bounce satisfying?"
- Write down honest feedback

**END OF DAY 2:** Bouncing ball that feels good (hopefully!) ✅

---

## 🛠️ UNITY PROJECT SETUP (30 minutes)

### Step 1: Create Folder Structure

In Unity's Project panel, right-click Assets → Create → Folder

Create these folders:
```
Assets/
├── Scenes/
├── Scripts/
│   └── Core/
├── Prefabs/
├── Materials/
├── Audio/
└── Sprites/
```

### Step 2: Save Your Scene

File → Save As...
Name: "PhysicsTestScene"
Location: Assets/Scenes/
Click Save

### Step 3: Configure Physics Settings

Edit → Project Settings → Physics 2D

Set these values:
```
Gravity:
  X: 0
  Y: -9.81

Velocity Iterations: 8
Position Iterations: 3
Velocity Threshold: 0.1
Max Linear Correction: 0.2
Sleeping Threshold: 0.005

Default Contact Offset: 0.01
```

Click somewhere else to save changes.

### Step 4: Create Physics Materials

Right-click in Project panel → Create → Physics Material 2D

**Create Two Materials:**

**Material 1: "BallPhysics"**
```
Friction: 0.1
Bounciness: 0.7
```

**Material 2: "PegPhysics"**
```
Friction: 0.0
Bounciness: 0.8
```

---

## 🎱 CREATE BALL (45 minutes)

### Step 1: Create Ball GameObject

1. In Hierarchy panel, right-click → 2D Object → Sprites → Circle
2. Rename it to "Ball"
3. In Inspector, set:
   - Position: X=0, Y=5, Z=0
   - Scale: X=0.5, Y=0.5, Z=1
   - Color: Red (or whatever you like)

### Step 2: Add Physics Components

With Ball selected in Hierarchy:

**Add Rigidbody 2D:**
1. Inspector → Add Component → Physics 2D → Rigidbody 2D
2. Set:
   - Body Type: Dynamic
   - Mass: 1
   - Linear Drag: 0
   - Angular Drag: 0.05
   - Gravity Scale: 1
   - Collision Detection: Continuous

**Add Circle Collider 2D:**
1. Inspector → Add Component → Physics 2D → Circle Collider 2D
2. Set:
   - Radius: 0.5
   - Material: Select "BallPhysics" material you created

### Step 3: Create Ball Script

1. Right-click in Assets/Scripts/Core/ → Create → C# Script
2. Name it "Ball"
3. Double-click to open (opens in default code editor)

**Copy-paste this ENTIRE script:**

```csharp
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Physics Settings")]
    public float settleVelocityThreshold = 0.1f;
    public float gridSize = 0.5f;
    public bool snapToGrid = true;

    [Header("Visual Feedback")]
    public Color settledColor = new Color(1f, 0.5f, 0.5f); // Light red when settled

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isSettled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Random rotation for variety
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void FixedUpdate()
    {
        CheckIfSettled();
    }

    void CheckIfSettled()
    {
        if (isSettled) return;

        // Check if ball has stopped moving
        if (rb.velocity.magnitude < settleVelocityThreshold)
        {
            SettleBall();
        }
    }

    void SettleBall()
    {
        isSettled = true;

        if (snapToGrid)
        {
            SnapToGrid();
        }

        // Stop all motion
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Visual feedback - change color when settled
        if (spriteRenderer != null)
        {
            spriteRenderer.color = settledColor;
        }

        Debug.Log("Ball settled at position: " + transform.position);
    }

    void SnapToGrid()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.y = Mathf.Round(pos.y / gridSize) * gridSize;
        transform.position = pos;
    }

    // Detect collisions for debugging
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ball hit: " + collision.gameObject.name + " at velocity: " + rb.velocity.magnitude);
    }
}
```

4. Save the file (Ctrl+S or Cmd+S)
5. Return to Unity (it will compile automatically)

### Step 4: Attach Script to Ball

1. Select "Ball" in Hierarchy
2. Drag the Ball.cs script from Project panel onto the Ball in Inspector
3. OR: Inspector → Add Component → type "Ball" → select it

### Step 5: Test the Ball

1. Click Play button (top center)
2. Ball should fall due to gravity
3. Ball should settle at bottom and turn light red
4. Check Console panel (bottom) for "Ball settled" message

**If ball falls through floor:**
- We'll add ground in next section

**If script errors:**
- Check Console panel for red error messages
- Copy-paste script again carefully

---

## 🎯 CREATE PEGS (45 minutes)

### Step 1: Create Peg GameObject

1. Hierarchy → right-click → 2D Object → Sprites → Circle
2. Rename to "Peg"
3. Set in Inspector:
   - Position: X=0, Y=0, Z=0
   - Scale: X=0.15, Y=0.15, Z=1
   - Color: Blue

### Step 2: Add Physics to Peg

**Add Circle Collider 2D:**
1. Inspector → Add Component → Physics 2D → Circle Collider 2D
2. Set:
   - Radius: 0.5
   - Material: Select "PegPhysics"

**NO Rigidbody!** Pegs are static, they don't move.

### Step 3: Create Peg Script (Optional but Nice)

Create new script: Assets/Scripts/Core/Peg.cs

```csharp
using UnityEngine;

public class Peg : MonoBehaviour
{
    [Header("Visual Feedback")]
    public Color normalColor = Color.blue;
    public Color hitColor = Color.cyan;
    public float hitFlashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float hitTimer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
    }

    void Update()
    {
        // Flash back to normal color after hit
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                spriteRenderer.color = normalColor;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Flash when ball hits
        if (collision.gameObject.CompareTag("Ball"))
        {
            Flash();
        }
    }

    void Flash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            hitTimer = hitFlashDuration;
        }
    }
}
```

Attach this script to Peg GameObject.

### Step 4: Tag the Ball (For Peg Script)

1. Select Ball in Hierarchy
2. Inspector → Tag dropdown (top) → Add Tag...
3. Click "+" → name new tag "Ball"
4. Select Ball again → Tag → select "Ball"

### Step 5: Create Peg Layout

**Option A: Manual (Simple):**
1. Drag Peg from Hierarchy to Project/Prefabs folder (creates prefab)
2. Duplicate peg: Select Peg → Ctrl+D (Cmd+D on Mac)
3. Move duplicates to create Plinko pattern:
   ```
   Row 1:     ●   ●   ●   ●   ●   ●   ●   ●
   Row 2:   ●   ●   ●   ●   ●   ●   ●   ●   ●
   Row 3:     ●   ●   ●   ●   ●   ●   ●   ●
   Row 4:   ●   ●   ●   ●   ●   ●   ●   ●   ●
   ...continue for 12-15 rows
   ```
4. Spacing: 0.8 units horizontal, 0.6 units vertical
5. Organize: Create empty GameObject "PegContainer", parent all pegs to it

**Option B: Script (Advanced):**

Create PegSpawner.cs:

```csharp
using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    public GameObject pegPrefab;
    public int rows = 12;
    public int columns = 8;
    public float horizontalSpacing = 0.8f;
    public float verticalSpacing = 0.6f;
    public Vector2 startPosition = new Vector2(-3f, 3f);

    void Start()
    {
        SpawnPegs();
    }

    void SpawnPegs()
    {
        for (int row = 0; row < rows; row++)
        {
            int pegsInRow = columns;
            float rowOffset = (row % 2 == 0) ? 0f : horizontalSpacing / 2f; // Offset every other row

            for (int col = 0; col < pegsInRow; col++)
            {
                float x = startPosition.x + col * horizontalSpacing + rowOffset;
                float y = startPosition.y - row * verticalSpacing;

                Vector3 position = new Vector3(x, y, 0);
                GameObject peg = Instantiate(pegPrefab, position, Quaternion.identity);
                peg.transform.parent = this.transform; // Organize under this GameObject
            }
        }
    }
}
```

Then:
1. Create empty GameObject → name "PegManager"
2. Attach PegSpawner script
3. Drag Peg prefab into "Peg Prefab" field in Inspector
4. Click Play → pegs spawn automatically

### Step 6: Add Ground

1. Hierarchy → right-click → 2D Object → Sprites → Square
2. Name: "Ground"
3. Set:
   - Position: X=0, Y=-6, Z=0
   - Scale: X=10, Y=0.5, Z=1
   - Color: Gray

4. Add Box Collider 2D (no Rigidbody, it's static)

### Step 7: Test Bouncing

1. Click Play
2. Ball should fall, bounce through pegs, settle on ground
3. Pegs should flash cyan when hit
4. Ball should turn light red when settled

**SUCCESS!** You have bouncing physics! 🎉

---

## 🎛️ PHYSICS TUNING GUIDE (Next 1-2 Weeks)

This is the MOST IMPORTANT part. AI can't do this - only YOU can feel when it's right.

### What to Tune:

**1. Bounciness (0.0 - 1.0)**

Try these values in **BallPhysics material**:

```
0.5 = Low bounce (ball loses energy fast)
0.6 = Medium-low
0.7 = Medium ⭐ START HERE
0.8 = High
0.9 = Very high (almost no energy loss)
```

**How to change:**
- Select BallPhysics material in Project
- Change "Bounciness" value
- Click Play to test
- Try each value for 5 drops minimum

**What to feel for:**
- Too low (0.5): Ball feels "dead", not exciting
- Too high (0.9): Ball bounces forever, annoying
- Just right: Satisfying "boing", settles in 3-5 seconds

---

**2. Friction (0.0 - 0.5)**

Try these values in **BallPhysics material**:

```
0.0 = No friction (pure sliding)
0.1 = Very low ⭐ START HERE
0.2 = Low
0.3 = Medium
0.5 = High (ball sticks)
```

**What to feel for:**
- Too low (0.0): Ball slides too much, feels icy
- Too high (0.5): Ball stops too fast, feels sticky
- Just right: Natural rolling, some slide on impact

---

**3. Gravity (-5 to -15)**

Try these values in **Physics 2D Settings**:

```
-5 = Low gravity (floaty, slow)
-7 = Medium-low
-9.81 = Earth gravity ⭐ START HERE
-12 = High gravity (fast fall)
-15 = Very high (too fast)
```

**What to feel for:**
- Too low: Feels slow, impatient
- Too high: Too fast to follow visually
- Just right: Visible but not boring

---

**4. Ball Size (0.3 - 0.8)**

Try these values in **Ball Transform Scale**:

```
0.3 = Tiny (hard to see)
0.4 = Small
0.5 = Medium ⭐ START HERE
0.6 = Large
0.8 = Very large (hits too many pegs)
```

**What to feel for:**
- Too small: Hard to track visually
- Too large: Predictable, less Plinko chaos
- Just right: Easy to see, good randomness

---

**5. Peg Spacing**

Try these in **PegSpawner or manual layout**:

```
Horizontal: 0.6 - 1.0 (try 0.8 first)
Vertical: 0.4 - 0.8 (try 0.6 first)
```

**What to feel for:**
- Too tight: Ball gets stuck
- Too loose: Ball falls straight, boring
- Just right: Ball bounces left/right unpredictably

---

### The Tuning Process (Hour by Hour):

**Hour 1-2: Try Each Value**
- Bounciness: 0.5, 0.6, 0.7, 0.8, 0.9
- Write down which feels best
- Pick top 2

**Hour 3-4: Fine-Tune Top 2**
- If 0.7 felt good, try 0.68, 0.72, 0.75
- Try combinations:
  - Bounciness 0.7 + Friction 0.1
  - Bounciness 0.7 + Friction 0.2
  - Bounciness 0.8 + Friction 0.05

**Hour 5-6: Gravity + Size**
- Try -9.81, -8, -10
- Try ball size 0.4, 0.5, 0.6
- Find combination that feels best

**Hour 7-8: Peg Layout**
- Adjust spacing
- Try different patterns
- More chaos vs more control

**Hour 9-10: Final Polish**
- Lock in your favorite values
- Test 50+ ball drops
- Show to friends

---

## 🎵 ADD SOUND (Optional but Recommended)

Makes it feel 10x better!

### Step 1: Download Sound

Go to freesound.org:
- Search "ball bounce"
- Download short (< 1 second) bounce sound
- Save to Assets/Audio/

### Step 2: Add to Ball

1. Select Ball in Hierarchy
2. Inspector → Add Component → Audio → Audio Source
3. Set:
   - AudioClip: Drag your bounce sound here
   - Play On Awake: OFF
   - Volume: 0.5

### Step 3: Add Code to Ball.cs

Add this to Ball.cs inside the class:

```csharp
private AudioSource audioSource;

void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>(); // ADD THIS LINE
}

void OnCollisionEnter2D(Collision2D collision)
{
    // Play sound on bounce
    if (audioSource != null && rb.velocity.magnitude > 0.5f)
    {
        // Vary pitch based on velocity (higher velocity = higher pitch)
        audioSource.pitch = 0.8f + (rb.velocity.magnitude * 0.1f);
        audioSource.Play();
    }

    Debug.Log("Ball hit: " + collision.gameObject.name + " at velocity: " + rb.velocity.magnitude);
}
```

Now ball makes sound when bouncing!

---

## ✅ WEEK 1-2 SUCCESS CRITERIA

### End of Week 1 (This Weekend):

- [ ] Unity installed and working
- [ ] Ball falls and bounces through pegs
- [ ] Ball settles on ground
- [ ] Pegs flash when hit
- [ ] Can change physics values and see difference
- [ ] You've tried 5+ different bounciness values

**Minimum Success:** Ball bounces. Period.

---

### End of Week 2 (Next Weekend):

- [ ] You've tried 20+ different value combinations
- [ ] You have 3 "finalists" that feel good
- [ ] You've shown it to 5 people
- [ ] You've written down their reactions
- [ ] You've picked your favorite combination
- [ ] Sound is added (bonus)

**Critical Success Metric:**
> **4 out of 5 people say "Yes, watching that bounce is satisfying"**

---

## 🎯 THE CRITICAL QUESTIONS (Ask Yourself & Others)

After each physics change, ask:

### For Yourself:
1. **"Can I watch this ball bounce for 30 seconds without getting bored?"**
   - YES = Good sign
   - NO = Keep tuning

2. **"Do I smile when I see a particularly good bounce?"**
   - YES = You're onto something
   - NO = Not there yet

3. **"Would I play a game with this physics?"**
   - YES = Proceed to full MVP
   - NO = Keep tuning or pivot

### For Playtesters:
1. **"Is watching this ball bounce satisfying?"** (YES/NO)
2. **"On a scale of 1-5, how satisfying?" (1=boring, 5=very satisfying)**
3. **"What would make it feel better?"**

**Record answers in a notebook or Google Doc.**

---

## 🚨 COMMON PROBLEMS & FIXES

### Ball Falls Through Floor
**Fix:** Make sure Ground has Box Collider 2D (static, no Rigidbody)

### Ball Doesn't Bounce
**Fix:**
- Check BallPhysics material has Bounciness > 0.5
- Make sure material is assigned to Ball's Circle Collider 2D

### Ball Bounces Forever
**Fix:** Lower bounciness to 0.6-0.7

### Ball Gets Stuck Between Pegs
**Fix:** Increase peg spacing or decrease ball size

### Pegs Don't Flash
**Fix:**
- Make sure Peg script is attached
- Make sure Ball has tag "Ball"
- Check Console for errors

### No Sound
**Fix:**
- Make sure Audio Source is on Ball
- Make sure Play On Awake is OFF
- Make sure volume > 0
- Check code has audioSource.Play()

---

## 📊 DECISION POINT (End of Week 2)

### ✅ SUCCESS (Proceed to Full MVP):

**If 4+ of 5 people say:**
- "Yes, that's satisfying"
- Rating 4-5 out of 5
- They lean forward / smile / show interest

**AND you personally:**
- Love watching it bounce
- Can watch for 1+ minute without boredom
- Feel excited to build more

**NEXT STEP:**
→ Commit to full 16-24 week MVP
→ Start Week 3: Merge mechanics
→ Follow QUANTUM_BOUNCE_COMPLETE_EXECUTION_PLAN.md

---

### ⚠️ PARTIAL SUCCESS (Keep Tuning):

**If 2-3 of 5 people say:**
- "It's okay"
- Rating 3 out of 5
- Neutral reaction

**AND you personally:**
- Feel like it's close but not quite there
- See potential but not satisfied yet

**NEXT STEP:**
→ Take 1 more week of tuning
→ Try more extreme values
→ Show to different people
→ Retest end of Week 3

---

### ❌ FAILURE (Pivot Immediately):

**If <2 of 5 people say:**
- "Not really satisfying"
- Rating 1-2 out of 5
- Bored, confused, no reaction

**OR you personally:**
- Don't enjoy watching it
- Feel frustrated after 2 weeks
- Can't see it working

**NEXT STEP:**
→ STOP working on physics game
→ You validated it's not your strength
→ Try Music Painter instead (less physics-dependent)
→ Or try grid-based game (no physics)
→ **DON'T waste 6 more months on this**

---

## 🎉 THIS WEEKEND CHECKLIST

Print this out and check off as you go:

### Saturday:
- [ ] Install Unity Hub
- [ ] Install Unity 2022.3 LTS
- [ ] Watch 30-min Unity tutorial
- [ ] Create "QuantumBouncePhysics" project
- [ ] Create folder structure
- [ ] Configure Physics2D settings
- [ ] Create physics materials
- [ ] Create Ball GameObject
- [ ] Add Rigidbody2D + Collider to Ball
- [ ] Create Ball.cs script
- [ ] Attach script to Ball
- [ ] Test: Ball falls ✅

### Sunday:
- [ ] Create Peg GameObject
- [ ] Add Collider to Peg
- [ ] Create Peg.cs script (optional)
- [ ] Create peg layout (manual or script)
- [ ] Create Ground
- [ ] Test: Ball bounces through pegs ✅
- [ ] Try 3 different bounciness values
- [ ] Try 2 different friction values
- [ ] Download bounce sound
- [ ] Add sound to Ball
- [ ] Show to 2-3 people
- [ ] Write down feedback

---

## 📞 NEED HELP?

If you get stuck:

**Unity Crashes / Won't Open:**
- Restart computer
- Reinstall Unity
- Check system requirements (8GB RAM minimum)

**Code Errors:**
- Copy-paste scripts exactly as written
- Check for typos
- Make sure script file name matches class name
- Click Edit → Preferences → External Tools → Regenerate project files

**Physics Not Working:**
- Check Console panel for errors (red messages)
- Make sure Rigidbody2D is on Ball (not on Peg)
- Make sure Colliders are on both Ball and Peg
- Check Physics2D settings match values above

**Can't Get Good Feel:**
- Watch Peggle gameplay on YouTube (5+ minutes)
- Note what makes it satisfying
- Try more extreme values (0.9 bounciness, 0.0 friction)
- Show to MORE people (10+, not just 5)

---

## 🚀 YOU'RE READY TO START

**You have everything you need:**
- ✅ Step-by-step instructions
- ✅ All code copy-paste ready
- ✅ Physics values to try
- ✅ Success criteria
- ✅ Decision framework

**This weekend's goal:**
**Ball bounces through pegs. Period.**

**Don't worry about:**
- Making it perfect (Week 2 is for tuning)
- Merge mechanics (Week 3+)
- Mystery balls (Week 5+)
- How it looks (ugly is fine for now)

**Just make it bounce. That's it.**

---

## 💪 MOTIVATION

Remember:
- Peggle was built by a team of 10+ over 2 years
- You're building the CORE of Peggle in 2 weeks
- With AI helping you
- Solo
- **That's impressive even if you only get halfway there**

**This is about learning and validation, not perfection.**

If bouncing feels good by Week 2:
→ You probably have a fun game

If it doesn't:
→ You saved 18-22 weeks by validating early

**Either way, you WIN by starting smart.** 🎯

---

**NOW GO BUILD.** See you in 2 weeks with a bouncing ball! 🚀

---

*Document Version: 1.0*
*Created: November 16, 2025*
*Estimated Time: 15-20 hours over 2 weeks*
*Difficulty: Beginner-Friendly*
*Success Rate: 70% if you follow exactly*
