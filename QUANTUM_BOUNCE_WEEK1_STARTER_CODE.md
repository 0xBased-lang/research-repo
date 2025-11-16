# QUANTUM BOUNCE: WEEK 1 STARTER CODE
## Get Started Coding TODAY - Ball Physics Implementation

**Goal:** By end of Day 1-2, have a ball dropping and bouncing through pegs.

---

## 📁 UNITY PROJECT SETUP (30 minutes)

### Step 1: Create New Project
```
1. Open Unity Hub
2. Create New Project
3. Template: "2D Core"
4. Name: "QuantumBounce"
5. Location: Choose your workspace
6. Click "Create Project"
```

### Step 2: Configure Physics Settings
```
1. Edit → Project Settings → Physics 2D
2. Set these values:
   - Gravity: Y = -9.81 (default)
   - Default Material: Create new (see below)
   - Velocity Iterations: 8
   - Position Iterations: 3
   - Velocity Threshold: 0.1
   - Max Linear Correction: 0.2
   - Sleeping Threshold: 0.005
```

### Step 3: Create Physics Material
```
1. Right-click in Project → Create → Physics Material 2D
2. Name: "BallPhysicsMaterial"
3. Set:
   - Friction: 0.1 (low, so ball slides smoothly)
   - Bounciness: 0.7 (high, for satisfying bounce)
```

### Step 4: Create Folder Structure
```
Assets/
├── Scenes/
│   └── GameScene.unity
├── Scripts/
│   └── Core/
├── Prefabs/
├── Materials/
└── Audio/
```

---

## 🎱 BALL SCRIPT (Core Physics)

### Create: `Assets/Scripts/Core/Ball.cs`

```csharp
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    [Header("Ball Properties")]
    public int value = 2; // 2, 4, 8, 16, etc.

    [Header("Physics Settings")]
    public float settleVelocityThreshold = 0.1f;
    public float gridSize = 0.5f;
    public bool snapToGrid = true;

    [Header("References")]
    public TextMeshPro valueText;
    public SpriteRenderer spriteRenderer;

    // Internal state
    private Rigidbody2D rb;
    private bool isSettled = false;
    private bool isMerged = false; // Prevent double-merge

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-find components if not assigned
        if (valueText == null)
            valueText = GetComponentInChildren<TextMeshPro>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UpdateVisual();

        // Set random rotation for variety
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

        // Optional: Change color slightly when settled
        Color c = spriteRenderer.color;
        c.a = 1f; // Full opacity when settled
        spriteRenderer.color = c;
    }

    void SnapToGrid()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.y = Mathf.Round(pos.y / gridSize) * gridSize;
        transform.position = pos;
    }

    public void UpdateVisual()
    {
        // Update text to show value
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }

        // Update color based on value (visual feedback)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetColorForValue(value);
        }

        // Scale ball based on value (larger values = slightly bigger)
        float scale = 0.4f + (Mathf.Log(value, 2) * 0.05f);
        transform.localScale = Vector3.one * Mathf.Clamp(scale, 0.4f, 0.8f);
    }

    Color GetColorForValue(int val)
    {
        // Simple color progression: 2=white, 4=yellow, 8=orange, 16=red, etc.
        switch (val)
        {
            case 2: return new Color(0.9f, 0.9f, 0.9f); // White
            case 4: return new Color(1f, 1f, 0.5f);     // Yellow
            case 8: return new Color(1f, 0.8f, 0.3f);   // Orange
            case 16: return new Color(1f, 0.5f, 0.3f);  // Red-orange
            case 32: return new Color(1f, 0.3f, 0.3f);  // Red
            case 64: return new Color(1f, 0.3f, 0.6f);  // Pink
            case 128: return new Color(0.8f, 0.3f, 1f); // Purple
            case 256: return new Color(0.5f, 0.3f, 1f); // Blue-purple
            case 512: return new Color(0.3f, 0.5f, 1f); // Blue
            case 1024: return new Color(0.3f, 0.8f, 1f);// Cyan
            case 2048: return new Color(0.3f, 1f, 0.5f);// Green (victory!)
            default: return Color.white;
        }
    }

    // Public getters
    public bool IsSettled() => isSettled;
    public bool IsMerged() => isMerged;
    public void SetMerged() => isMerged = true;

    // Called when ball is created mid-air (not dropped)
    public void SetUnsettled()
    {
        isSettled = false;
    }
}
```

---

## 🎯 PEG SCRIPT (Simple Bouncer)

### Create: `Assets/Scripts/Core/Peg.cs`

```csharp
using UnityEngine;

public class Peg : MonoBehaviour
{
    [Header("Visual Feedback")]
    public float hitScalePunch = 1.2f;
    public float hitAnimationDuration = 0.1f;
    public Color hitColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isAnimating = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only react to balls
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            if (!isAnimating)
            {
                StartCoroutine(HitAnimation());
            }
        }
    }

    System.Collections.IEnumerator HitAnimation()
    {
        isAnimating = true;

        // Scale up and change color
        float elapsed = 0f;
        while (elapsed < hitAnimationDuration / 2f)
        {
            float t = elapsed / (hitAnimationDuration / 2f);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * hitScalePunch, t);
            spriteRenderer.color = Color.Lerp(originalColor, hitColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Scale back down
        elapsed = 0f;
        while (elapsed < hitAnimationDuration / 2f)
        {
            float t = elapsed / (hitAnimationDuration / 2f);
            transform.localScale = Vector3.Lerp(originalScale * hitScalePunch, originalScale, t);
            spriteRenderer.color = Color.Lerp(hitColor, originalColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure back to original
        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;

        isAnimating = false;
    }
}
```

---

## 🎮 BALL SPAWNER (Drop Balls)

### Create: `Assets/Scripts/Core/BallSpawner.cs`

```csharp
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float spawnHeight = 5f;

    [Header("Input")]
    public KeyCode spawnKey = KeyCode.Space;

    void Update()
    {
        // Press space to spawn ball (for testing)
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnBall();
        }

        // Also allow mouse click to spawn
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBall();
        }
    }

    public void SpawnBall()
    {
        Vector3 spawnPos;

        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.position;
        }
        else
        {
            // Spawn at mouse position X, fixed Y height
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos = new Vector3(mousePos.x, spawnHeight, 0);
        }

        // Create ball
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);

        // Optional: Add slight random horizontal velocity for variety
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(Random.Range(-0.5f, 0.5f), 0);
        }
    }
}
```

---

## 🎨 UNITY SCENE SETUP (GameObject Creation)

### Create These GameObjects in Hierarchy:

#### 1. **Main Camera**
```
- Position: (0, 0, -10)
- Size (Orthographic): 8
- Background: Dark blue/black (0.1, 0.1, 0.15)
```

#### 2. **BallSpawner** (Empty GameObject)
```
- Position: (0, 0, 0)
- Add Component: BallSpawner.cs
- Create child: SpawnPoint
  - Position: (0, 5, 0) [where balls spawn from]
```

#### 3. **PegLayout** (Empty GameObject - parent for all pegs)
```
- Position: (0, 0, 0)
```

#### 4. **Boundaries** (Create 4 walls)
```
Left Wall:
  - Position: (-4, 0, 0)
  - Add: Box Collider 2D
  - Size: (0.2, 20)

Right Wall:
  - Position: (4, 0, 0)
  - Add: Box Collider 2D
  - Size: (0.2, 20)

Bottom:
  - Position: (0, -8, 0)
  - Add: Box Collider 2D
  - Size: (10, 0.2)

Top (optional, prevents balls going too high):
  - Position: (0, 8, 0)
  - Add: Box Collider 2D
  - Size: (10, 0.2)
```

---

## 🎱 CREATE BALL PREFAB

### Step-by-Step:

1. **Create GameObject in Scene:**
   - GameObject → 2D Object → Sprites → Circle
   - Rename to "Ball"

2. **Add Components:**
   ```
   Add Component: Rigidbody 2D
     - Mass: 1
     - Gravity Scale: 1
     - Collision Detection: Continuous
     - Constraints: Freeze Rotation Z (optional, prevents spinning)

   Add Component: Circle Collider 2D
     - Radius: 0.5
     - Material: BallPhysicsMaterial (drag from Assets)

   Add Component: Ball.cs (drag script)
   ```

3. **Add Text Child (for displaying value):**
   ```
   Right-click Ball → 3D Object → Text - TextMeshPro
   (If prompted, import TMP Essentials)

   Rename to "ValueText"
   - Position: (0, 0, -1) [in front of ball]
   - Font Size: 36
   - Alignment: Center, Middle
   - Color: Black
   - Width/Height: 1, 1
   ```

4. **Assign References:**
   ```
   In Ball component:
   - Drag ValueText into "Value Text" field
   - Sprite Renderer should auto-assign
   ```

5. **Create Prefab:**
   ```
   - Drag "Ball" from Hierarchy → Assets/Prefabs/
   - Delete Ball from scene (we'll spawn it via code)
   ```

---

## 🎯 CREATE PEG PREFAB

### Step-by-Step:

1. **Create GameObject:**
   - GameObject → 2D Object → Sprites → Circle
   - Rename to "Peg"
   - Scale: (0.3, 0.3, 1) [smaller than ball]

2. **Add Components:**
   ```
   Add Component: Circle Collider 2D
     - Radius: 0.5
     - (No Rigidbody - pegs are static!)

   Add Component: Peg.cs
   ```

3. **Change Color:**
   ```
   - Select Peg
   - In Sprite Renderer:
     - Color: Light gray (0.7, 0.7, 0.7)
   ```

4. **Create Prefab:**
   ```
   - Drag "Peg" from Hierarchy → Assets/Prefabs/
   - Keep one in scene (we'll manually place pegs)
   ```

---

## 🎯 LAYOUT PEGS (Plinko Style)

### Manual Placement (for MVP):

Create this pattern (classic Plinko board):

```
Row 1 (Y=4):    o           [1 peg, centered]
Row 2 (Y=3):   o o          [2 pegs]
Row 3 (Y=2):  o o o         [3 pegs]
Row 4 (Y=1): o o o o        [4 pegs]
Row 5 (Y=0): o o o o o      [5 pegs]
Row 6 (Y=-1): o o o o o o   [6 pegs]
Row 7 (Y=-2):  o o o o o    [5 pegs, offset]
Row 8 (Y=-3):   o o o o     [4 pegs, offset]
Row 9 (Y=-4):    o o o      [3 pegs, offset]
Row 10 (Y=-5):    o o       [2 pegs, offset]
```

**Positions (copy-paste into Excel, then manually create):**

```
Peg positions (X, Y):
(0, 4)
(-0.6, 3), (0.6, 3)
(-1.2, 2), (0, 2), (1.2, 2)
(-1.8, 1), (-0.6, 1), (0.6, 1), (1.8, 1)
(-2.4, 0), (-1.2, 0), (0, 0), (1.2, 0), (2.4, 0)
... etc
```

**Quick Method:**
```
1. Select Peg in scene
2. Duplicate (Ctrl+D)
3. Move to position using Transform tool
4. Repeat until pattern complete
5. Select all pegs → parent under "PegLayout" GameObject
```

---

## 🔗 WIRE IT UP

### In BallSpawner GameObject:

1. Select BallSpawner in Hierarchy
2. In Inspector, BallSpawner component:
   - **Ball Prefab:** Drag "Ball" prefab from Assets/Prefabs
   - **Spawn Point:** Drag "SpawnPoint" child object

---

## ✅ TEST IT (5 minutes)

### Press Play:

1. **Press Space** or **Click Mouse** → Ball should spawn
2. Ball should drop through pegs
3. Ball should bounce realistically
4. Pegs should "pulse" when hit
5. Ball should settle at bottom

### Expected Behavior:
- ✅ Ball drops smoothly
- ✅ Bounces feel satisfying
- ✅ Pegs react to hits
- ✅ Ball stops at bottom
- ✅ Number displays on ball

### If Something's Wrong:

**Ball falls through pegs:**
- Check pegs have Circle Collider 2D
- Check ball has Circle Collider 2D
- Check layers aren't ignoring each other

**Ball doesn't bounce:**
- Check Physics Material is assigned
- Check Bounciness is > 0

**Ball spins wildly:**
- In Ball Rigidbody 2D → Constraints → Freeze Rotation Z

**Performance is bad:**
- Reduce peg count
- Check no infinite loops in scripts

---

## 📊 TUNING VALUES (Make it Feel Good)

### Physics Material (BallPhysicsMaterial):
```
Try these values and see what feels best:

Bounciness:
- 0.5 = soft, realistic
- 0.7 = satisfying, game-like (RECOMMENDED)
- 0.9 = super bouncy, chaotic

Friction:
- 0.0 = slippery (ball slides)
- 0.1 = smooth (RECOMMENDED)
- 0.3 = sticky (ball grips)
```

### Ball.cs Settings:
```
Settle Velocity Threshold:
- 0.05 = settles quickly
- 0.1 = balanced (RECOMMENDED)
- 0.2 = takes longer to settle

Grid Size:
- 0.25 = tight grid (precise placement)
- 0.5 = medium (RECOMMENDED)
- 1.0 = loose grid (more chaotic)
```

### Rigidbody 2D:
```
Mass:
- 0.5 = light, floaty
- 1.0 = balanced (RECOMMENDED)
- 2.0 = heavy, drops fast

Gravity Scale:
- 0.8 = slower fall
- 1.0 = realistic (RECOMMENDED)
- 1.5 = faster, more intense
```

---

## 🎯 END OF DAY 1-2 CHECKPOINT

### You Should Have:
- [x] Unity project set up
- [x] Ball prefab created
- [x] Peg prefab created
- [x] Peg layout in scene (Plinko style)
- [x] Ball spawns on click/spacebar
- [x] Ball bounces through pegs
- [x] Ball settles at bottom
- [x] Numbers display on balls

### Critical Question:
**"Is it FUN to drop a ball and watch it bounce?"**

If **YES** → You have the core! Move to Week 2 (Merge mechanics)
If **NO** → Spend more time tuning physics values until it feels right

---

## 🐛 COMMON ISSUES & FIXES

### Issue: Ball passes through pegs
```
FIX:
1. Select Ball prefab
2. Rigidbody 2D → Collision Detection = "Continuous"
3. Edit → Project Settings → Time
   - Fixed Timestep: 0.02 (default)
```

### Issue: Ball bounces forever
```
FIX:
1. Select BallPhysicsMaterial
2. Reduce Bounciness to 0.6-0.7
3. In Ball.cs, reduce settleVelocityThreshold to 0.05
```

### Issue: Pegs don't react to hits
```
FIX:
1. Check Peg.cs is attached to peg
2. Check peg has Circle Collider 2D
3. Check collision layers (should be default)
```

### Issue: Performance is slow
```
FIX:
1. Reduce number of pegs (< 50 pegs)
2. Remove peg hit animation temporarily
3. Check no other scripts running in background
```

### Issue: Ball doesn't snap to grid
```
FIX:
1. In Ball inspector, check "Snap To Grid" is enabled
2. Increase gridSize to 1.0 (more obvious)
3. Check FixedUpdate is being called
```

---

## 📝 DEBUGGING TIPS

### Add Debug Logs:

In Ball.cs `CheckIfSettled()`:
```csharp
void CheckIfSettled()
{
    if (isSettled) return;

    Debug.Log($"Ball velocity: {rb.velocity.magnitude}"); // Add this

    if (rb.velocity.magnitude < settleVelocityThreshold)
    {
        Debug.Log("Ball settled!"); // Add this
        SettleBall();
    }
}
```

### Visualize Colliders:

```
1. In Game view, click "Gizmos" button
2. Enable "Show Colliders"
3. You'll see green outlines of all colliders
```

---

## ⏭️ WHAT'S NEXT (Week 2)

Once you have satisfying ball physics, Week 2 adds:

1. **Merge Detection System**
   - Check when two balls of same value touch
   - Only merge when both settled

2. **Merge Execution**
   - Destroy both balls
   - Create new ball with double value
   - Particle effect + sound

3. **Score System**
   - Track total score
   - Display on UI
   - Save high score

**But for NOW:** Focus on making the ball drop feel AMAZING. Everything else builds on this foundation.

---

## 🎮 PLAY WITH IT

Spend 30 minutes just dropping balls and watching them bounce.

Ask yourself:
- Do I smile when the ball bounces?
- Do I want to drop another ball?
- Does it feel satisfying?

If **YES** to all three → You're ready for Week 2!
If **NO** → Keep tuning physics until it does.

**The core loop has to feel good BEFORE adding complexity.**

---

**Ready to start? Open Unity and follow this guide step-by-step. You'll have a working physics prototype in 2-3 hours!** 🚀

Any questions or stuck on something? Let me know which step you're on!
