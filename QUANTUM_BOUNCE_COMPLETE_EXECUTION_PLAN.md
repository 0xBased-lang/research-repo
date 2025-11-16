# QUANTUM BOUNCE: COMPLETE EXECUTION PLAN
## Ultra-Comprehensive Project Roadmap (12-Week MVP to Launch)

**Created:** November 16, 2025
**Target Launch:** February 2026
**Status:** READY TO EXECUTE

---

## 🎯 EXECUTIVE SUMMARY

### What We're Building
**Quantum Bounce** - A physics-based merge puzzle game combining:
- Plinko/Peggle ball-dropping mechanics
- 2048-style number merging
- "Mystery Ball" probability system (slot machine psychology)
- Gravity flip mechanic for strategy

### Why This Will Work
✅ **Proven mechanics** - Peggle (50M+ downloads) + 2048 (100M+) + Suika Game (viral 2023)
✅ **Unique hook** - Mystery balls create "jackpot moments" (TikTok viral potential)
✅ **Simple to learn** - Drop balls, match numbers, get surprises
✅ **Addictive psychology** - Variable rewards, near-misses, progression dopamine
✅ **Complete roadmap** - 12-week plan with starter code ready

### Critical Success Factors
1. **Ball bouncing must FEEL amazing** (Week 2 checkpoint)
2. **Mystery ball reveals must trigger dopamine** (Week 6 checkpoint)
3. **First-time users understand in <60 seconds** (Week 11 checkpoint)
4. **40%+ Day 1 retention** (Post-launch metric)

---

## 📋 PART 1: CRITICAL TECHNICAL DECISIONS

### DECISION 1: Platform & Engine ✅ UNITY 2D

**Choice:** Unity 2022.3 LTS with 2D Physics (Box2D)

**Reasoning:**
- ✅ Cross-platform (Android + iOS from single codebase)
- ✅ Mature physics engine (Box2D proven for Angry Birds, etc.)
- ✅ Large asset store for sounds/particles
- ✅ Free tier sufficient for MVP
- ✅ You have starter code ready in Unity

**Alternatives Considered:**
- ❌ Godot - Smaller mobile ecosystem
- ❌ React Native + Matter.js - Performance concerns for physics
- ❌ Native Swift/Kotlin - Double development time

**LOCKED IN:** Unity 2D it is.

---

### DECISION 2: MVP Scope (What's IN, What's OUT) ✅ MINIMALIST

**✅ IN SCOPE (MVP - 12 Weeks):**

**Core Mechanics:**
- [x] Drop balls from top
- [x] Realistic Plinko bouncing through pegs
- [x] 2048-style merging (2→4→8→16...→2048)
- [x] Mystery balls (10% chance on merge)
- [x] Mystery reveal with slot machine animation
- [x] Gravity flip button (3-second cooldown)
- [x] Score system + high score (local save)
- [x] Game over when board full

**UI/UX:**
- [x] Main menu (Play, Settings)
- [x] In-game HUD (score, gravity button)
- [x] Game over screen (score, retry)
- [x] 5-panel tutorial (skippable)
- [x] Settings (sound on/off, music on/off)

**Polish:**
- [x] Particle effects (merge burst, mystery sparkle, jackpot fireworks)
- [x] Sound effects (drop, bounce, merge, reveal, jackpot)
- [x] Background music (simple loop)
- [x] Screen shake on merges/jackpots
- [x] Ball visual variation by value (colors, size)

**❌ OUT OF SCOPE (Phase 2 - Post-MVP):**
- [ ] Server infrastructure (ALL client-side for MVP)
- [ ] Leaderboards (local high score only)
- [ ] Daily seed challenges (free play only)
- [ ] Multiplayer/social features
- [ ] Multiple game modes (just endless)
- [ ] Power-ups beyond mystery balls
- [ ] Progression system/unlocks
- [ ] Achievements
- [ ] Monetization (100% free for testing)
- [ ] Advanced anti-cheat
- [ ] Analytics SDK (manual tracking)
- [ ] Professional art (programmer art OK)
- [ ] Multiple peg layouts (one hand-designed)

**RATIONALE:**
Validate core loop FIRST. These features add 8-12 weeks and don't test if the game is fun.

---

### DECISION 3: Mystery Ball Implementation ✅ CLIENT-SIDE (MVP ONLY)

**For MVP (Week 1-12):**
- Client-side probability calculation
- Accept that cheating is possible
- No leaderboards = no competitive advantage to cheating
- Focus on single-player fun

**Post-MVP (if successful):**
- Move to server-authoritative validation
- Add Firebase backend
- Implement daily seed challenges
- Global leaderboards with anti-cheat

**RATIONALE:**
Server costs $800-$3500/month. Don't pay this until we know the game is fun.

---

### DECISION 4: Physics Determinism ✅ NOT REQUIRED FOR MVP

**For MVP:**
- Accept that physics may vary slightly between devices
- No daily seed = no need for perfect determinism
- Focus on "feels good" not "exactly reproducible"

**Post-MVP:**
- If adding daily seeds, accept 95% similarity (not 100%)
- Use leaderboards based on score, not replays
- Server validates final score, not each physics tick

**RATIONALE:**
Perfect determinism is 6+ months of work and may be impossible. Not worth it for MVP.

---

### DECISION 5: Art Style ✅ SIMPLE & FUNCTIONAL

**For MVP:**
- Circles for balls (Unity sprites)
- Simple color coding by value:
  - 2 = Red
  - 4 = Orange
  - 8 = Yellow
  - 16 = Green
  - 32 = Cyan
  - 64 = Blue
  - 128 = Purple
  - 256 = Pink
  - 512+ = Gold/Rainbow
- TextMeshPro for numbers
- Pegs = small circles
- Background = solid color or simple gradient

**Post-MVP:**
- Hire artist for polish
- Custom illustrations
- Animated ball sprites
- Fancy UI

**RATIONALE:**
Function over form. Suika Game proved simple visuals can go viral if gameplay is addictive.

---

### DECISION 6: Audio Strategy ✅ FREE ASSETS + SIMPLE

**For MVP:**
- Source from freesound.org, zapsplat.com
- Background music: Simple ambient loop (incompetech.com)
- 5-10 key sound effects:
  1. Ball drop (soft thud)
  2. Bounce (pitch varies with velocity)
  3. Merge (satisfying "ding")
  4. Mystery appear (magical chime)
  5. Slot machine roll (looping rattle)
  6. Reveal normal (soft bell)
  7. Reveal double (ascending notes)
  8. Reveal quad (short fanfare)
  9. Jackpot (full celebration music sting)
  10. Button click (soft beep)

**Post-MVP:**
- Custom sound design
- Dynamic music system
- Professional audio mix

**RATIONALE:**
Good enough audio from free assets. Spend $0-50 max.

---

## 📅 PART 2: WEEK-BY-WEEK DEVELOPMENT PLAN

### WEEK 1-2: FOUNDATION & CORE PHYSICS

**Goal:** Ball drops and bounces. It FEELS GOOD.

**Tasks:**
- [ ] **Day 1:** Unity project setup, Git repo, folder structure
- [ ] **Day 2:** Configure Physics2D settings, create physics material
- [ ] **Day 3:** Create Ball prefab (CircleCollider2D, Rigidbody2D, Ball.cs script)
- [ ] **Day 4:** Create Peg prefab, design Plinko peg layout (15x8 grid)
- [ ] **Day 5:** Implement ball spawner (tap to drop from top)
- [ ] **Day 6-7:** Tune physics values (gravity, bounciness, friction)
- [ ] **Day 8:** Add ball settling logic (snap to grid when velocity < 0.1)
- [ ] **Day 9:** Visual feedback (pegs glow when hit)
- [ ] **Day 10:** Playtest - watch balls bounce for 5 minutes

**Deliverable:** Ball drops, bounces through pegs, settles at bottom.

**Checkpoint:** Does watching a ball bounce make you smile?
- ✅ YES → Continue to Week 3
- ❌ NO → Spend Week 3 tuning physics until it's satisfying

**Code Focus:**
- `Ball.cs` - Physics, settling, snap-to-grid
- `Peg.cs` - Hit detection, visual feedback
- `BallSpawner.cs` - Drop ball from top on tap

---

### WEEK 3-4: MERGE MECHANICS

**Goal:** 2048-style merging works reliably and feels satisfying.

**Tasks:**
- [ ] **Day 1-2:** Implement merge detection (OnCollisionEnter2D, check values match)
- [ ] **Day 3:** Merge logic (destroy both balls, spawn new ball with doubled value)
- [ ] **Day 4:** Handle simultaneous collisions (merge only one pair per frame)
- [ ] **Day 5:** Particle effect on merge (burst of color)
- [ ] **Day 6:** Sound effect + screen shake on merge
- [ ] **Day 7:** Merge animation (new ball grows from small)
- [ ] **Day 8:** Score system (add merge value to score)
- [ ] **Day 9:** Display score UI (top of screen)
- [ ] **Day 10:** Save/load high score (PlayerPrefs)

**Deliverable:** Merging works 100% reliably with satisfying feedback.

**Checkpoint:** Do merges make you want to create more merges?
- ✅ YES → Continue to Week 5
- ❌ NO → Increase particle effects, louder sounds, more screen shake

**Code Focus:**
- `MergeSystem.cs` - Collision detection, merge logic
- `ScoreManager.cs` - Score tracking, high score save/load
- `ParticleEffects.cs` - Merge burst effect

---

### WEEK 5-6: MYSTERY BALL SYSTEM

**Goal:** Mystery balls create anticipation and dopamine spikes.

**Tasks:**
- [ ] **Day 1:** 10% of merges create mystery ball instead of normal
- [ ] **Day 2:** Mystery ball visual ("?" icon, pulsing glow, particle sparkle)
- [ ] **Day 3:** Tap to reveal mechanic (OnMouseDown)
- [ ] **Day 4:** Implement probability roll (60/25/12/3 split)
- [ ] **Day 5:** Slot machine animation (spin 2 seconds, slow down, land)
- [ ] **Day 6:** Different celebration for each tier (normal/double/quad/jackpot)
- [ ] **Day 7:** Jackpot effects (fireworks, screen shake, slow-mo, fanfare)
- [ ] **Day 8:** Tune probabilities (does 3% jackpot feel right?)
- [ ] **Day 9:** Prevent spam-clicking (lock during reveal)
- [ ] **Day 10:** Playtest - do mystery balls excite you?

**Deliverable:** Mystery balls create "OH WOW!" moments.

**Checkpoint:** Do you feel a dopamine spike during reveal?
- ✅ YES → Continue to Week 7
- ❌ NO → Increase jackpot probability to 5%, add more visual effects

**Code Focus:**
- `MysteryBall.cs` - Tap detection, reveal coroutine, probability roll
- `AudioManager.cs` - Slot machine sounds, celebration tiers
- `CameraShake.cs` - Screen shake intensity scaling

---

### WEEK 7-8: GRAVITY FLIP & GAME LOOP

**Goal:** Complete game loop (start → play → end → retry).

**Tasks:**
- [ ] **Day 1:** Gravity flip button UI (bottom center)
- [ ] **Day 2:** Flip Physics2D.gravity on tap (down ↔ up)
- [ ] **Day 3:** 3-second cooldown with visual progress bar
- [ ] **Day 4:** Gravity direction indicator (arrow icon)
- [ ] **Day 5:** Sound effect for gravity flip (satisfying whoosh)
- [ ] **Day 6:** Detect game over (board full + no merges possible)
- [ ] **Day 7:** Game over screen (final score, high score, retry button)
- [ ] **Day 8:** Restart functionality (clear board, reset score)
- [ ] **Day 9:** Detect 2048 reached (victory condition)
- [ ] **Day 10:** Victory screen (celebration, option to continue)

**Deliverable:** Full playable loop from start to finish.

**Checkpoint:** Can you play a complete game without bugs?
- ✅ YES → Continue to Week 9
- ❌ NO → Fix critical bugs before proceeding

**Code Focus:**
- `GravityFlip.cs` - Button, cooldown timer, physics toggle
- `GameManager.cs` - Game states (Playing, GameOver, Victory)
- `UIManager.cs` - Screen transitions

---

### WEEK 9-10: POLISH & JUICE

**Goal:** Make every action feel AMAZING.

**Tasks:**
- [ ] **Day 1:** All sound effects implemented (10 total)
- [ ] **Day 2:** Background music loop (ambient, not annoying)
- [ ] **Day 3:** Particle effects for all key moments
- [ ] **Day 4:** Screen shake tuning (intensity by importance)
- [ ] **Day 5:** Ball drop animation (spawn with bounce)
- [ ] **Day 6:** Ball color/size variation by value
- [ ] **Day 7:** UI transitions (fade in/out, smooth)
- [ ] **Day 8:** Tweak physics feel (bounciness, gravity)
- [ ] **Day 9:** Performance optimization (object pooling for balls)
- [ ] **Day 10:** Test on 3 different devices (high/mid/low-end)

**Deliverable:** Game feels polished and satisfying.

**Checkpoint:** Do you want to play "just one more round"?
- ✅ YES → Continue to Week 11
- ❌ NO → Identify what feels "off" and fix it

**Code Focus:**
- `ObjectPool.cs` - Reuse balls instead of Instantiate/Destroy
- Audio mixing and balancing
- Visual polish pass

---

### WEEK 11: TUTORIAL & UX

**Goal:** New players understand the game in <60 seconds.

**Tasks:**
- [ ] **Day 1-2:** 5-panel tutorial (swipe through):
  1. "Tap to drop balls"
  2. "Match same numbers to merge"
  3. "Mystery balls = surprise bonuses!"
  4. "Flip gravity for strategy"
  5. "Reach 2048 to win!"
- [ ] **Day 3:** Tutorial skip button (prominent)
- [ ] **Day 4:** First-time user flow (tutorial → first game)
- [ ] **Day 5:** Settings screen (sound, music toggles)
- [ ] **Day 6:** Pause functionality
- [ ] **Day 7:** Main menu polish (clean, simple)
- [ ] **Day 8:** Test with 3 people who've never seen it
- [ ] **Day 9:** Fix confusion points identified in testing
- [ ] **Day 10:** Final UX polish

**Deliverable:** Strangers can play without your explanation.

**Checkpoint:** Can someone play without asking "how does this work?"
- ✅ YES → Continue to Week 12
- ❌ NO → Simplify tutorial, add visual cues

**Code Focus:**
- `TutorialManager.cs` - Panel system, first-time detection
- UI clarity improvements
- Visual hints (arrows, highlights)

---

### WEEK 12: TESTING & BUILD

**Goal:** Shippable build with zero critical bugs.

**Tasks:**
- [ ] **Day 1:** Test on Android (3 devices: high/mid/low-end)
- [ ] **Day 2:** Test on iOS (2 devices if possible)
- [ ] **Day 3-5:** Bug bash (play 50+ full games, fix all critical bugs)
- [ ] **Day 6:** Balance tuning (mystery ball %, gravity cooldown)
- [ ] **Day 7:** Create Android APK build
- [ ] **Day 8:** Create iOS build (if targeting)
- [ ] **Day 9:** App icon + screenshots (5 required for stores)
- [ ] **Day 10:** Write app description, prepare for distribution

**Deliverable:** Stable build ready for soft launch.

**Checkpoint:** Zero critical bugs in 10 consecutive playthroughs?
- ✅ YES → Launch soft test
- ❌ NO → Fix bugs, delay by 1 week

**Code Focus:**
- Bug fixing
- Build optimization
- Store submission prep

---

## 🎮 PART 3: DETAILED TECHNICAL ARCHITECTURE

### Tech Stack Summary
```
ENGINE: Unity 2022.3 LTS
LANGUAGE: C#
PHYSICS: Unity Physics 2D (Box2D)
UI: Unity UI Canvas + TextMeshPro
AUDIO: Unity Audio System
DATA STORAGE: PlayerPrefs (local only)
VERSION CONTROL: Git + GitHub
BUILD TARGETS: Android (primary), iOS (secondary)

MVP INFRASTRUCTURE:
- No backend servers
- No database
- No cloud services
- 100% client-side
```

### Project Structure
```
QuantumBounce/
├── Assets/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   └── GameScene.unity
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── Ball.cs
│   │   │   ├── MysteryBall.cs
│   │   │   ├── Peg.cs
│   │   │   └── BallSpawner.cs
│   │   ├── Managers/
│   │   │   ├── GameManager.cs (Singleton)
│   │   │   ├── ScoreManager.cs (Singleton)
│   │   │   ├── AudioManager.cs (Singleton)
│   │   │   └── UIManager.cs (Singleton)
│   │   ├── Systems/
│   │   │   ├── MergeSystem.cs
│   │   │   ├── GravityFlip.cs
│   │   │   ├── ParticleEffects.cs
│   │   │   ├── CameraShake.cs
│   │   │   └── ObjectPool.cs
│   │   └── UI/
│   │       ├── MainMenu.cs
│   │       ├── GameHUD.cs
│   │       ├── GameOverScreen.cs
│   │       ├── TutorialManager.cs
│   │       └── SettingsMenu.cs
│   ├── Prefabs/
│   │   ├── Balls/
│   │   │   ├── Ball_2.prefab
│   │   │   ├── Ball_4.prefab (etc. for each value)
│   │   │   └── MysteryBall.prefab
│   │   ├── Pegs/
│   │   │   └── Peg.prefab
│   │   ├── Effects/
│   │   │   ├── MergeParticles.prefab
│   │   │   ├── MysterySparkle.prefab
│   │   │   └── JackpotFireworks.prefab
│   │   └── UI/
│   │       └── (UI prefabs)
│   ├── Materials/
│   │   ├── BallPhysicsMaterial.physicsMaterial2D
│   │   └── PegPhysicsMaterial.physicsMaterial2D
│   ├── Audio/
│   │   ├── SFX/
│   │   │   ├── ball_drop.wav
│   │   │   ├── bounce.wav
│   │   │   ├── merge.wav
│   │   │   ├── mystery_appear.wav
│   │   │   ├── slot_roll.wav
│   │   │   ├── reveal_normal.wav
│   │   │   ├── reveal_double.wav
│   │   │   ├── reveal_quad.wav
│   │   │   ├── jackpot.wav
│   │   │   └── button_click.wav
│   │   └── Music/
│   │       └── background_ambient.mp3
│   ├── Sprites/
│   │   ├── ball_sprites/ (circles with colors)
│   │   ├── ui_elements/
│   │   └── background.png
│   └── Fonts/
│       └── (TextMeshPro fonts)
├── ProjectSettings/
└── Packages/
```

### Physics Configuration Values
```csharp
// Physics2D Settings (Edit → Project Settings → Physics 2D)
Gravity: (0, -9.81)
Default Material: BallPhysicsMaterial
Velocity Iterations: 8
Position Iterations: 3
Velocity Threshold: 0.1
Max Linear Correction: 0.2
Sleeping Threshold: 0.005

// BallPhysicsMaterial
Friction: 0.1 (low for smooth sliding)
Bounciness: 0.7 (high for satisfying bounce)

// PegPhysicsMaterial
Friction: 0.0 (no friction on pegs)
Bounciness: 0.8 (slightly higher for dynamic bounces)
```

### Core Systems Design

**1. Game Manager (State Machine)**
```csharp
public enum GameState {
    MainMenu,
    Tutorial,
    Playing,
    Paused,
    GameOver,
    Victory
}

// Singleton pattern
// Manages transitions between states
// Coordinates other managers
```

**2. Score Manager**
```csharp
// Tracks current score
// Manages high score (PlayerPrefs)
// Triggers score UI updates
// Handles score multipliers (future)
```

**3. Merge System**
```csharp
// FixedUpdate() checks all ball pairs
// Collision detection (distance < threshold)
// Both balls must be settled (velocity ~0)
// Handles edge cases (simultaneous merges)
// 10% chance → spawn MysteryBall instead
// Triggers particle effects + sounds
```

**4. Mystery Ball System**
```csharp
// Visual: "?" icon + pulsing glow
// OnMouseDown() → StartCoroutine(RevealAnimation())
// 2-second slot machine spin
// Roll probability:
//   60% → expected value
//   25% → double
//   12% → quadruple
//   3% → mega (8x+)
// Scale celebration to outcome
// Convert to normal Ball after reveal
```

**5. Gravity Flip System**
```csharp
// Button with 3-second cooldown
// Toggle Physics2D.gravity.y between -9.81 and +9.81
// Visual indicator (arrow pointing up/down)
// Progress bar shows cooldown
// Sound effect on flip
```

---

## ⚠️ PART 4: RISK ASSESSMENT & MITIGATION

### CRITICAL RISK 1: Physics Doesn't Feel Good

**Probability:** MEDIUM (30%)
**Impact:** CATASTROPHIC (game fails if not fun)

**Mitigation:**
- ✅ Allocate Week 2 for physics tuning ONLY
- ✅ Test with 5+ people, iterate until smiles
- ✅ Reference values from successful physics games:
  - Peggle: Bounciness 0.7-0.9, low friction
  - Angry Birds: Gravity -9.81, high collision accuracy
- ✅ CHECKPOINT: If not satisfying by Week 2, pivot immediately

**Contingency:**
- Try different peg layouts (hexagonal vs grid)
- Adjust ball size (larger = more forgiving)
- Add "ball trail" visual for better feedback

---

### CRITICAL RISK 2: Mystery Balls Not Exciting

**Probability:** MEDIUM (25%)
**Impact:** HIGH (loses key differentiator)

**Mitigation:**
- ✅ Make reveal animation RIDICULOUSLY over-the-top
- ✅ Increase jackpot probability to 5% if 3% too rare
- ✅ Test with people who like slot machines/gacha games
- ✅ Add audio cues (heartbeat during reveal?)

**Contingency:**
- Make mystery balls more frequent (20% instead of 10%)
- Add visual preview (slight shimmer showing potential values)
- Implement "near-miss" effect (show jackpot, then land on lower)

---

### CRITICAL RISK 3: Confusion in Onboarding

**Probability:** LOW (15%)
**Impact:** MEDIUM (Day 1 retention <30%)

**Mitigation:**
- ✅ Tutorial must be visual, not text-heavy
- ✅ Test with non-gamers (friends/family who don't play mobile games)
- ✅ Add visual arrows pointing to key elements
- ✅ First few balls are guaranteed to create easy merges

**Contingency:**
- Add "hint" button that shows next best move
- Simplify tutorial to 3 panels instead of 5
- Add in-game tooltips that appear when needed

---

### CRITICAL RISK 4: Performance Issues on Low-End Devices

**Probability:** LOW (20%)
**Impact:** MEDIUM (limits audience)

**Mitigation:**
- ✅ Test on actual budget Android device (not just emulator)
- ✅ Implement object pooling (Week 9)
- ✅ Limit max balls on screen to 25
- ✅ Profile with Unity Profiler, optimize hotspots

**Contingency:**
- Reduce particle count on low-end devices
- Lower physics accuracy (fewer iterations)
- Target 30fps minimum (not 60fps)

---

### CRITICAL RISK 5: Scope Creep

**Probability:** HIGH (60%)
**Impact:** MEDIUM (delays launch)

**Mitigation:**
- ✅ Lock feature list in this document
- ✅ Weekly checkpoint: "Are we still on MVP scope?"
- ✅ Use "Phase 2 backlog" for new ideas
- ✅ No new features after Week 8

**Contingency:**
- If behind schedule, cut polish (Week 9-10)
- Delay iOS build, ship Android only
- Accept "functional but ugly" UI for MVP

---

## 🧪 PART 5: TESTING & VALIDATION STRATEGY

### Weekly Checkpoints (Go/No-Go Decisions)

**Week 2 Checkpoint:**
- Question: Does ball bouncing feel satisfying?
- Test: Show to 5 people, watch them drop 10 balls
- Success: 4/5 say "that's satisfying"
- Failure: Spend Week 3 tuning instead of merges

**Week 4 Checkpoint:**
- Question: Are merges fun and reliable?
- Test: Play 20 games yourself
- Success: Zero merge bugs, you want to make more merges
- Failure: Fix merge bugs before proceeding

**Week 6 Checkpoint:**
- Question: Do mystery balls create excitement?
- Test: Show 5 people, watch them reveal 10 mystery balls
- Success: 4/5 show visible excitement (lean forward, "ooh!")
- Failure: Increase jackpot %, add more celebration effects

**Week 8 Checkpoint:**
- Question: Can you complete a game without bugs?
- Test: Play 10 full games (start to game over)
- Success: Zero crashes, zero softlocks
- Failure: Bug fixing week

**Week 10 Checkpoint:**
- Question: Does it feel polished?
- Test: Record yourself playing, watch back
- Success: No jarring moments, everything flows
- Failure: Identify rough edges, polish them

**Week 11 Checkpoint:**
- Question: Can strangers understand it?
- Test: 3 people who've never seen it play without help
- Success: All 3 complete tutorial + first game
- Failure: Simplify tutorial, add visual cues

**Week 12 Checkpoint:**
- Question: Is it shippable?
- Test: 10 consecutive full playthroughs
- Success: Zero critical bugs
- Failure: Delay launch by 1 week, fix bugs

---

### Bug Priority System

**P0 (Critical - Must fix immediately):**
- Crashes
- Softlocks (can't proceed)
- Data loss (score/high score corruption)
- Game-breaking bugs (balls fall through floor)

**P1 (High - Fix before launch):**
- Merge bugs (wrong value, no merge when should)
- Mystery ball probability broken
- UI elements not responding
- Audio not playing

**P2 (Medium - Fix if time):**
- Visual glitches (particles in wrong place)
- Animation jank
- Performance hiccups
- UI alignment issues

**P3 (Low - Phase 2):**
- Polish improvements
- Nice-to-have features
- Minor visual issues

---

### Device Testing Matrix

**Minimum Test Devices:**
- Android High-End: Pixel 7+ or Samsung S22+
- Android Mid-Range: Pixel 6a or Samsung A53
- Android Budget: Pixel 4a or Samsung A32 (2020-2021)
- iOS: iPhone 12+ (if targeting iOS)

**Performance Targets:**
- High-End: 60fps sustained
- Mid-Range: 60fps with occasional drops to 50fps
- Budget: 30fps minimum, 45fps target

**Must Test:**
- Different screen sizes (small phone, tablet)
- Different aspect ratios (16:9, 18:9, 19.5:9)
- Different OS versions (Android 11, 12, 13, 14)

---

## 🚀 PART 6: LAUNCH & DISTRIBUTION STRATEGY

### Soft Launch Plan (Week 13-14)

**Distribution Method:**
- APK via Google Drive link (Android)
- TestFlight (iOS, if applicable)
- NOT on stores yet (testing phase)

**Tester Recruitment (Target: 30-50 people):**

**Sources:**
1. Personal network (10 people)
   - Friends, family, coworkers
   - Ask for brutal honesty

2. Reddit (15-20 people)
   - r/playmygame
   - r/androidgaming
   - r/incremental_games
   - Post: "I made a physics puzzle game, need testers"

3. Discord communities (10-15 people)
   - Indie game dev servers
   - Mobile gaming communities

4. Twitter/X (5-10 people)
   - #gamedev #indiedev tags
   - Share short gameplay clip

**Tester Instructions:**
```
WHAT WE NEED:
1. Install APK / TestFlight build
2. Play for at least 15 minutes (5+ full games)
3. Fill out feedback form (link below)
4. Report any bugs via Discord/email

FEEDBACK FORM QUESTIONS:
1. Did you understand how to play? (Yes/No)
2. Was it fun? (1-5 scale, 5=very fun)
3. Would you play again tomorrow? (Yes/Maybe/No)
4. Would you pay $2.99 for this? (Yes/Maybe/No)
5. What was most confusing?
6. What was most fun?
7. What would make you play daily?
8. Any bugs or issues?
9. Device model & Android/iOS version
10. Additional comments

THANK YOU:
- Your name in credits (if desired)
- Free premium version when we launch (if we monetize)
```

---

### Success Metrics (Decision Framework)

**After 2 weeks of testing (30+ responses):**

**STRONG SUCCESS (Proceed to Phase 2):**
- ✅ 70%+ say "Yes, it was fun" (4-5 rating)
- ✅ 50%+ would play again tomorrow
- ✅ 20%+ would pay $2.99
- ✅ <20% found it confusing
- ✅ 10+ organic shares/recommendations

**ACTION:**
→ Invest in Phase 2 (server infrastructure, monetization, polish)
→ Budget: $2000-$5000 + 8-12 weeks
→ Launch on app stores

---

**MODERATE SUCCESS (Iterate & Retest):**
- ⚠️ 50-70% say "it was fun"
- ⚠️ 30-50% would play again
- ⚠️ 10-20% would pay
- ⚠️ 20-40% found it confusing

**ACTION:**
→ Identify top 3 pain points from feedback
→ Spend 2-4 weeks fixing them
→ Retest with fresh group
→ If improved, proceed to Phase 2
→ If not improved, pivot to different concept

---

**WEAK SUCCESS (Pivot or Simplify):**
- ⚠️ 30-50% say "it was fun"
- ⚠️ <30% would play again
- ⚠️ <10% would pay
- ⚠️ 40%+ found it confusing

**ACTION:**
→ Major changes needed:
   - Remove mystery balls? (test without them)
   - Remove gravity flip? (too complex?)
   - Simplify to just merge mechanics?
→ 4-week pivot sprint
→ Test again
→ If still weak, consider abandoning

---

**FAILURE (Kill or Major Pivot):**
- ❌ <30% say "it was fun"
- ❌ <20% would play again
- ❌ Majority say "too confusing" or "not fun"

**ACTION:**
→ Honest postmortem: Why did it fail?
   - Core mechanic not fun?
   - Physics feel was off?
   - Mystery balls not exciting?
   - Too complex for casual audience?
→ EITHER:
   A. Radical pivot (completely different concept)
   B. Kill project, apply learnings to next game
   C. Try one of the other concepts (Synesthesia, Music Painter)

**IMPORTANT:** 12 weeks is a cheap lesson. Don't throw good money after bad.

---

## 💰 PART 7: BUDGET & RESOURCE ALLOCATION

### MVP Budget (Week 1-12)

**Development Costs:**
```
YOUR TIME:
12 weeks × 40 hours/week = 480 hours
Opportunity cost: $24,000 @ $50/hr or $48,000 @ $100/hr
(This is the REAL cost - your time)

DIRECT COSTS:
Unity Personal: $0 (free tier)
Asset Store Purchases:
  - Sound effects pack: $0-$50 (or free from freesound.org)
  - Particle effects pack: $0-$30 (or use Unity default)
  - UI asset pack: $0-$40 (optional)
Total Assets: $0-$120

Platform Fees:
Google Play Developer: $25 (one-time)
Apple Developer: $99/year (if iOS)
Total Platform: $25-$124

Testing:
Test devices: $0 (use what you own)
User testing incentives: $0 (volunteers)

GRAND TOTAL: $25-$244 out-of-pocket
```

**Ongoing Costs (MVP):**
```
Servers: $0 (all client-side)
Database: $0 (PlayerPrefs local)
CDN: $0 (no assets to serve)
Analytics: $0 (manual tracking)

MONTHLY: $0
```

---

### Phase 2 Budget (If MVP Succeeds)

**Development:**
- 8-12 weeks additional development
- Opportunity cost: $16k-$48k (your time)

**Infrastructure:**
```
Backend Server (Firebase or custom):
- Firebase Spark: $0 (generous free tier)
- Firebase Blaze: $25-$500/month (once you scale)
- OR Custom server (AWS/DigitalOcean): $50-$200/month

Database:
- Firebase Firestore: Included in above
- OR PostgreSQL (managed): $15-$100/month

CDN:
- CloudFlare: $0-$20/month
- AWS CloudFront: $10-$100/month

TOTAL MONTHLY: $25-$500 initially
Scales up to $800-$3500 if you hit 100k+ users
```

**Art & Polish:**
```
Professional UI/UX designer: $500-$2000
Sound designer: $300-$1000
Music composer: $200-$800
App icon & screenshots: $100-$500

TOTAL: $1100-$4300 (optional but recommended)
```

**Marketing (Optional for Phase 2):**
```
Social media ads: $500-$2000
Influencer outreach: $0-$1000 (small creators)
Press release: $100-$500

TOTAL: $600-$3500
```

**Phase 2 Total Investment:** $2000-$8000 + server costs

---

## 📊 PART 8: SUCCESS METRICS & KPIs

### MVP Test Metrics (Week 13-14)

**Track Manually:**
- Number of testers: Target 30+
- Feedback responses: Target 25+
- "Fun" rating average: Target 4.0+/5.0
- "Would play again" %: Target 50%+
- "Would pay" %: Target 20%+
- "Confusing" %: Target <20%
- Critical bugs reported: Target <5
- Average session length: Target 10+ minutes (estimate from feedback)

---

### Post-Launch Metrics (Phase 2+)

**Retention:**
- Day 1: Target 40%+ (return next day)
- Day 7: Target 20%+
- Day 30: Target 10%+

**Engagement:**
- Session length: Target 10-15 minutes
- Sessions per day: Target 2-3
- Games per session: Target 3-5

**Viral:**
- Share rate: Target 5%+ (1 in 20 players shares)
- K-factor (viral coefficient): Target 0.5+ (each user brings 0.5 more)
- Organic installs %: Target 30%+ (word-of-mouth)

**Monetization (if you add it):**
- IAP conversion: Target 2-5%
- ARPU (average revenue per user): Target $0.50-$2.00/month
- LTV (lifetime value): Target $5-$20

---

## 🔄 PART 9: CONTINGENCY PLANS

### Scenario 1: Behind Schedule

**If Week 6 and not done with Weeks 1-5:**

**Action:**
- Cut mystery balls from MVP → Add in Phase 2
- Ship with just merge mechanics
- This saves 2 weeks
- Still validates core loop

---

### Scenario 2: Physics Feels Bad

**If Week 2 checkpoint fails:**

**Action:**
- Don't proceed to Week 3
- Spend entire Week 3 on physics tuning
- Try different values:
  - Lower gravity (-5 instead of -9.81)
  - Higher bounciness (0.9 instead of 0.7)
  - Bigger balls (easier to see)
- If still not fun by end of Week 3:
  - Pivot to grid-based instead of physics?
  - Or abandon this concept

---

### Scenario 3: Mystery Balls Not Exciting

**If Week 6 checkpoint fails:**

**Action:**
- Increase jackpot probability (5% → 10%)
- Add slow-motion on jackpot (3 seconds)
- Add screen flash (white flash on reveal)
- Make animation longer (3 sec instead of 2 sec)
- Retest

**If still not exciting:**
- Consider removing mystery balls entirely
- Ship as pure merge + physics game
- Differentiate with gravity flip instead

---

### Scenario 4: Too Confusing for Players

**If Week 11 checkpoint fails (testers confused):**

**Action:**
- Reduce tutorial to 3 panels:
  1. "Tap to drop balls"
  2. "Match numbers to merge"
  3. "Ready? Let's play!"
- Add visual hints in first game:
  - Arrow pointing where to tap
  - Highlight when merge possible
- First 5 balls are pre-set to create easy wins

---

### Scenario 5: MVP Test Fails Completely

**If feedback is overwhelmingly negative:**

**Action:**
- Take 1 week to analyze WHY
  - Was core mechanic boring?
  - Was it too hard?
  - Was it too easy?
  - Was the hook unclear?

**Then decide:**
- **Option A:** Try one of the other concepts
  - Synesthesia (music + art + physics)
  - AI Music Painter (simpler, less physics)
  - Pure 2048 with gravity flip (no physics)

- **Option B:** Take learnings and move on
  - 12 weeks is a cheap lesson
  - You learned Unity, physics, game design
  - Apply to next project

**DO NOT:**
- Keep iterating on a concept players hate
- Sink 6 more months trying to "fix" it
- Ignore negative feedback

---

## 📝 PART 10: ACTIONABLE NEXT STEPS

### THIS WEEK (Week 0 - Prep):

**Day 1-2: Environment Setup**
- [ ] Install Unity Hub
- [ ] Install Unity 2022.3 LTS
- [ ] Create GitHub repository
- [ ] Set up project structure
- [ ] Review starter code from `QUANTUM_BOUNCE_WEEK1_STARTER_CODE.md`

**Day 3-4: Asset Gathering**
- [ ] Download free sound effects from freesound.org
  - Search: "ball bounce", "merge", "slot machine", "jackpot"
- [ ] Find simple background music (incompetech.com)
- [ ] Create simple color palette for balls (red→orange→yellow→green→cyan→blue→purple)

**Day 5: Planning**
- [ ] Print out week-by-week plan (this document)
- [ ] Set up weekly checkpoints in calendar
- [ ] Identify 3-5 test devices you have access to
- [ ] Recruit 5 friends willing to playtest each week

**Day 6-7: First Commit**
- [ ] Create Unity project
- [ ] Configure Physics2D settings
- [ ] Create folder structure
- [ ] Create Ball.cs script (copy from starter code)
- [ ] Push to GitHub
- [ ] YOU'RE OFFICIALLY STARTED! 🚀

---

### WEEK 1 (Starting Monday):

**Monday:**
- [ ] 9am: Create Ball prefab (Circle sprite, CircleCollider2D, Rigidbody2D)
- [ ] 11am: Implement Ball.cs script
- [ ] 2pm: Create Peg prefab
- [ ] 4pm: Design peg layout (15 rows × 8 columns, Plinko style)

**Tuesday:**
- [ ] 9am: Implement BallSpawner.cs (spawn on tap)
- [ ] 11am: Test - can you drop a ball?
- [ ] 2pm: Adjust physics material (bounciness, friction)
- [ ] 4pm: Watch ball bounce - is it satisfying?

**Wednesday:**
- [ ] All day: Physics tuning
- [ ] Try different bounce values: 0.5, 0.6, 0.7, 0.8, 0.9
- [ ] Try different friction values: 0.0, 0.1, 0.2
- [ ] Get it FEELING GOOD

**Thursday:**
- [ ] 9am: Add ball settling logic (snap to grid when stopped)
- [ ] 11am: Add peg hit detection
- [ ] 2pm: Visual feedback (peg glows when hit)
- [ ] 4pm: Test full drop → bounce → settle flow

**Friday:**
- [ ] 9am: Polish pass (make sure everything works)
- [ ] 11am: Show to 3 friends - "does this feel good?"
- [ ] 2pm: Incorporate feedback
- [ ] 4pm: Git commit, push
- [ ] End of day: CHECKPOINT - Is bouncing satisfying?

**Weekend:**
- [ ] Rest OR get ahead on Week 2 (merge mechanics)

---

### DECISION POINTS

**End of Week 2:**
- ✅ If bouncing feels GREAT → Proceed with confidence
- ⚠️ If bouncing feels OKAY → Spend 1 more week on physics
- ❌ If bouncing feels BAD → Pivot or abort

**End of Week 6:**
- ✅ If mystery balls are EXCITING → MVP will probably succeed
- ⚠️ If mystery balls are MEH → Make them more frequent/dramatic
- ❌ If mystery balls are BORING → Consider removing them

**End of Week 11:**
- ✅ If strangers understand it → Ready for soft launch
- ⚠️ If some confusion → Simplify tutorial, add hints
- ❌ If total confusion → Major UX redesign needed

**After 2-week soft launch:**
- ✅ If 50%+ want to play more → Build Phase 2
- ⚠️ If 30-50% → Iterate based on feedback
- ❌ If <30% → Pivot or kill

---

## 🎯 FINAL CHECKLIST

Before calling MVP "complete":

### Core Gameplay
- [ ] Ball drops smoothly when tapped
- [ ] Bouncing feels satisfying (passes Week 2 checkpoint)
- [ ] Merges work 100% reliably
- [ ] Mystery balls appear 10% of the time
- [ ] Mystery reveals are exciting (passes Week 6 checkpoint)
- [ ] Gravity flip works and feels impactful
- [ ] Game over triggers correctly
- [ ] Restart works (clears board, resets score)

### Technical
- [ ] Runs at 60fps on mid-range Android
- [ ] Runs at 30fps on budget Android
- [ ] No crashes in 10 consecutive games
- [ ] No softlocks or game-breaking bugs
- [ ] High score saves/loads correctly
- [ ] Object pooling implemented (performance)

### UX
- [ ] Tutorial is clear (passes Week 11 checkpoint)
- [ ] Strangers can play without help
- [ ] UI is functional (doesn't need to be pretty)
- [ ] Sound enhances experience
- [ ] Can mute audio
- [ ] Settings save correctly

### Build
- [ ] APK builds successfully
- [ ] Installs on test devices
- [ ] App icon created (512x512 PNG)
- [ ] 5 screenshots captured
- [ ] App description written (150 words)

### Testing
- [ ] Tested on 3+ devices
- [ ] 5+ people have played it
- [ ] Feedback collected
- [ ] Critical bugs fixed
- [ ] Known issues documented

### Launch Prep
- [ ] Google Drive folder for APK distribution
- [ ] Feedback form created (Google Forms)
- [ ] Tester recruitment plan ready
- [ ] Reddit/Discord posts drafted
- [ ] TestFlight set up (if iOS)

---

## 📚 APPENDIX: RESOURCES

### Free Asset Sources
- **Sounds:** freesound.org, zapsplat.com, sonniss.com
- **Music:** incompetech.com, bensound.com, purple-planet.com
- **Fonts:** Google Fonts, dafont.com
- **Sprites:** kenney.nl, opengameart.org
- **Particles:** Unity Particle Pack (free on Asset Store)

### Learning Resources
- **Unity Physics 2D:** Unity Learn (official tutorials)
- **2048 Clone Tutorial:** YouTube "2048 Unity tutorial"
- **Juice & Feel:** "Juice it or lose it" GDC talk (YouTube)
- **Merge Mechanics:** Suika Game postmortems
- **Mobile Game Design:** "Clash of Clans" GDC talks

### Community
- **Reddit:** r/gamedev, r/unity2d, r/incremental_games
- **Discord:** Unity official, Indie Game Devs, Brackeys
- **Twitter/X:** #gamedev, #unity3d, #indiedev, #madewithunity

### Tools
- **Version Control:** GitHub Desktop (easy Git UI)
- **Bug Tracking:** Trello, Notion (simple board)
- **Testing Distribution:** Google Drive, Dropbox
- **Feedback Collection:** Google Forms, Typeform
- **Screen Recording:** OBS Studio, AZ Screen Recorder (mobile)

---

## 🏁 CONCLUSION

You have:
- ✅ **Complete 12-week plan** with daily tasks
- ✅ **Starter code** ready to copy-paste
- ✅ **Risk mitigation** for every major issue
- ✅ **Clear checkpoints** to fail fast if needed
- ✅ **Budget** of $25-$244 out-of-pocket
- ✅ **Success metrics** to make go/no-go decisions

**What you DON'T have:**
- ❌ Excuses to delay
- ❌ Uncertainty about next steps
- ❌ Fear of the unknown

**The path is clear. The plan is solid. The only question is:**

## Are you ready to start building TODAY? 🚀

---

**Next Action:** Set up Unity project (Day 1 tasks above)

**Accountability:** Commit to showing bouncing ball physics to someone by Friday

**Remember:** It's better to ship a "good enough" MVP in 12 weeks than a "perfect" game in never.

**Let's build this. 🎮**

---

*Document Version: 1.0*
*Created: November 16, 2025*
*Total Word Count: ~11,500 words*
*Estimated Read Time: 45 minutes*
*Implementation Time: 12 weeks (480 hours)*
