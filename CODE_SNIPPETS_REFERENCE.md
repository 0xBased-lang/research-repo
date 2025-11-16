# CODE SNIPPETS - QUICK REFERENCE
## Copy-Paste Ready Scripts for Physics MVP

---

## 📄 Ball.cs (Complete)

**Location:** `Assets/Scripts/Core/Ball.cs`

```csharp
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Physics Settings")]
    public float settleVelocityThreshold = 0.1f;
    public float gridSize = 0.5f;
    public bool snapToGrid = true;

    [Header("Visual Feedback")]
    public Color settledColor = new Color(1f, 0.5f, 0.5f);

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isSettled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void FixedUpdate()
    {
        CheckIfSettled();
    }

    void CheckIfSettled()
    {
        if (isSettled) return;

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

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play sound on bounce
        if (audioSource != null && rb.velocity.magnitude > 0.5f)
        {
            audioSource.pitch = 0.8f + (rb.velocity.magnitude * 0.1f);
            audioSource.Play();
        }

        Debug.Log("Ball hit: " + collision.gameObject.name + " at velocity: " + rb.velocity.magnitude);
    }
}
```

---

## 📄 Peg.cs (Complete)

**Location:** `Assets/Scripts/Core/Peg.cs`

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

---

## 📄 PegSpawner.cs (Optional - Auto-generates peg layout)

**Location:** `Assets/Scripts/Core/PegSpawner.cs`

```csharp
using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    [Header("Peg Settings")]
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
            float rowOffset = (row % 2 == 0) ? 0f : horizontalSpacing / 2f;

            for (int col = 0; col < pegsInRow; col++)
            {
                float x = startPosition.x + col * horizontalSpacing + rowOffset;
                float y = startPosition.y - row * verticalSpacing;

                Vector3 position = new Vector3(x, y, 0);
                GameObject peg = Instantiate(pegPrefab, position, Quaternion.identity);
                peg.transform.parent = this.transform;
            }
        }
    }
}
```

---

## 📄 BallSpawner.cs (Click to spawn balls)

**Location:** `Assets/Scripts/Core/BallSpawner.cs`

```csharp
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ballPrefab;
    public Vector2 spawnPosition = new Vector2(0, 5);
    public KeyCode spawnKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnBall();
        }

        // Also spawn on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBallAtMouse();
        }
    }

    void SpawnBall()
    {
        if (ballPrefab != null)
        {
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnBallAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (ballPrefab != null)
        {
            Instantiate(ballPrefab, mousePos, Quaternion.identity);
        }
    }
}
```

---

## ⚙️ Physics Settings (Quick Reference)

### Physics2D Settings
**Edit → Project Settings → Physics 2D**

```
Gravity: (0, -9.81)
Velocity Iterations: 8
Position Iterations: 3
Velocity Threshold: 0.1
Max Linear Correction: 0.2
Sleeping Threshold: 0.005
```

### BallPhysics Material
```
Friction: 0.1
Bounciness: 0.7
```

### PegPhysics Material
```
Friction: 0.0
Bounciness: 0.8
```

---

## 🎨 GameObject Setup (Quick Reference)

### Ball GameObject
```
Components:
- Sprite Renderer (Circle, Scale: 0.5)
- Rigidbody 2D (Dynamic, Mass: 1)
- Circle Collider 2D (Radius: 0.5, Material: BallPhysics)
- Ball.cs script
- Audio Source (optional)

Tag: "Ball"
```

### Peg GameObject
```
Components:
- Sprite Renderer (Circle, Scale: 0.15)
- Circle Collider 2D (Radius: 0.5, Material: PegPhysics)
- Peg.cs script

NO Rigidbody (static object)
```

### Ground GameObject
```
Components:
- Sprite Renderer (Square, Scale X: 10, Y: 0.5)
- Box Collider 2D

NO Rigidbody (static object)
Position: (0, -6, 0)
```

---

## 🎛️ Values to Try (Tuning Guide)

### Bounciness (BallPhysics material)
```
Try: 0.5, 0.6, 0.7, 0.8, 0.9
Start: 0.7
Sweet spot usually: 0.65 - 0.75
```

### Friction (BallPhysics material)
```
Try: 0.0, 0.1, 0.2, 0.3
Start: 0.1
Sweet spot usually: 0.05 - 0.15
```

### Gravity (Physics2D settings)
```
Try: -7, -9.81, -12
Start: -9.81 (Earth gravity)
Sweet spot usually: -8 to -11
```

### Ball Size (Ball Transform Scale)
```
Try: 0.4, 0.5, 0.6
Start: 0.5
Sweet spot usually: 0.45 - 0.55
```

### Peg Spacing (PegSpawner or manual)
```
Horizontal: Try 0.6 - 1.0 (start 0.8)
Vertical: Try 0.4 - 0.8 (start 0.6)
```

---

## 🐛 Common Errors & Fixes

### "The name 'rb' does not exist"
**Fix:** Make sure `rb` is declared: `private Rigidbody2D rb;`

### "Object reference not set to an instance"
**Fix:** Make sure component exists before using:
```csharp
if (rb != null) { /* use rb */ }
```

### Ball doesn't bounce
**Fix:**
- Check BallPhysics material has Bounciness > 0
- Make sure material is assigned to Circle Collider 2D

### Ball falls through floor
**Fix:** Make sure Ground has Collider (Box Collider 2D)

### No sound
**Fix:**
- Audio Source on Ball
- Play On Awake = OFF
- Volume > 0
- Audio clip assigned

---

## 📋 Weekend Checklist (Print This)

### Saturday (6-7 hours):
- [ ] Install Unity 2022.3 LTS
- [ ] Create project "QuantumBouncePhysics"
- [ ] Create folders (Scenes, Scripts/Core, Prefabs, Materials, Audio, Sprites)
- [ ] Configure Physics2D settings
- [ ] Create BallPhysics material (Friction 0.1, Bounciness 0.7)
- [ ] Create PegPhysics material (Friction 0.0, Bounciness 0.8)
- [ ] Create Ball GameObject (Sprite, Rigidbody2D, CircleCollider2D)
- [ ] Create Ball.cs script (copy from above)
- [ ] Attach Ball script to Ball
- [ ] Test: Ball falls ✅

### Sunday (5-6 hours):
- [ ] Create Peg GameObject (Sprite, CircleCollider2D)
- [ ] Create Peg.cs script (copy from above)
- [ ] Create peg layout (manual or PegSpawner)
- [ ] Create Ground (Sprite, BoxCollider2D)
- [ ] Test: Ball bounces ✅
- [ ] Try 5 different bounciness values
- [ ] Download bounce sound from freesound.org
- [ ] Add AudioSource to Ball
- [ ] Test sound works
- [ ] Show to 2-3 people
- [ ] Write down feedback

---

## 🎯 Success Criteria

**End of Weekend:**
- Ball drops and bounces through pegs
- Ball settles on ground
- Pegs flash when hit
- Can change physics values

**End of Week 2:**
- Tried 20+ value combinations
- 4 of 5 people say "satisfying"
- You personally love the feel
- Ready to decide: Continue or pivot?

---

**KEEP THIS OPEN WHILE CODING** 📌

All scripts above are complete and copy-paste ready.
No modifications needed to get started.

Good luck! 🚀
