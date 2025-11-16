# SubScout - Executive Summary & Decision Framework

**Date**: November 2025
**Status**: Research Complete - Decision Required

---

## TL;DR - Critical Insights

### ✅ What's Feasible
- Plaid integration for automatic subscription detection (~90% accuracy)
- React Native + Supabase provides fast MVP development (4-6 months)
- Market demand is real ($100+/month average spending, 42% forget subscriptions)
- Basic ML for churn prediction is achievable

### ⚠️ Major Challenges Uncovered
1. **"One-tap cancellation" is NOT possible** - requires human concierge service
2. **Plaid costs are MUCH higher than expected** - $500-1,500/month minimum
3. **Break-even requires 15,000+ users** (not 1,000 as projected)
4. **Compliance costs**: $12K-70K in Year 1
5. **FTC Click to Cancel rule does NOT help** - recently voided, never created APIs

### 💰 Revised Financial Reality

| Original Projection | Reality Check |
|---------------------|---------------|
| Month 6: $250 MRR with 1,000 users | **Plaid alone costs $750/month** → -$500 loss |
| Month 12: $2,000 MRR with 5,000 users | **Operating costs $8,600/month** → -$6,600 loss |
| Passive income with minimal support | **Concierge service requires customer support team** |
| Free tier business model | **$500 Plaid minimum makes true freemium impossible** |

---

## The Brutal Truth: 3 Critical Gaps

### 1. The Cancellation Automation Myth

**What the pitch claimed**:
> "One-tap cancellation: Built-in cancellation flows (leveraging FTC rules)"

**What's actually possible**:
- FTC Click to Cancel rule was **voided in 2025** by federal court
- Even when active, it NEVER required APIs or third-party access
- Automated web scraping faces massive legal risks (CFAA violations, ToS breaches)
- Rocket Money uses **human concierge teams**, not automation
- Each cancellation takes 15-30 minutes of human labor ($6-12 cost)

**Impact**: The core "magic" feature doesn't exist. You're building a concierge service, not automation.

---

### 2. The Economics Don't Work at Small Scale

**Plaid Pricing Reality**:
```
Pay-as-you-go: $1.50 per user per month
Minimum Growth tier: $500/month
Break-even point: ~12,000-15,000 users minimum

Your projection:
Month 6: 1,000 users × 5% conversion × $4.99 = $250 revenue
Month 6: 1,000 users × $1.50 Plaid cost = $1,500 expenses
NET: -$1,250/month (before any other costs!)
```

**The Problem**: Plaid costs alone exceed projected revenue until 15,000+ users

**Solutions**:
1. Raise $300-500K seed funding to cover 18-24 months of losses
2. Start with manual transaction entry (no Plaid) until validation
3. Charge $7.99-14.99/month (not $4.99) and target 15%+ conversion
4. Explore B2B2C model (sell to banks who absorb Plaid costs)

---

### 3. "Passive Income" Requires Active Support

**The pitch**: "Minimal support needed (self-service)"

**The reality**:
- Concierge cancellations require customer support agents
- $15-25/hour × 15-30 min per cancellation = $6.25-12.50 cost
- If 400 Premium users request 2 cancellations/month = 800 requests = $5,000-10,000/month in labor
- This is NOT passive income

**Options**:
1. Limit concierge to 2-3 cancellations per month per user
2. Charge premium pricing ($14.99/month) to cover labor costs
3. Focus on "instructions + links" as free tier (low value but scalable)

---

## What You Need to Decide

### Option A: Build It (With Revised Plan)

**Requirements**:
- [ ] Raise $300K-500K seed funding
- [ ] Accept 18-24 month path to break-even
- [ ] Pivot messaging from "one-tap" to "concierge service"
- [ ] Increase pricing to $7.99-14.99/month
- [ ] Build customer support team infrastructure
- [ ] Target 15,000-25,000 users for viability

**Best For**: Founders with VC connections, high tolerance for capital intensity, belief in long-term market size

---

### Option B: Bootstrap Path

**Approach**:
1. **Phase 1 (2-3 months)**: Build without Plaid
   - Manual transaction entry
   - Manual subscription tracking
   - Validate willingness to pay $7.99/month
   - Target: 100 paying users = $800 MRR

2. **Phase 2 (if Phase 1 succeeds)**: Add Plaid
   - Integrate after proving concept
   - Negotiate Plaid pricing with user traction
   - Hire first support agent for concierge

**Best For**: Bootstrappers, risk-averse founders, those wanting validation before capital investment

---

### Option C: Pivot the Concept

#### Pivot 1: B2B2C White-Label
**Model**: Sell subscription tracking solution to banks/credit unions
- They pay Plaid costs (already have banking relationships)
- You provide white-label UI + concierge service
- Revenue: $5-15K/month per bank partner

#### Pivot 2: Affiliate-First Model
**Model**: Focus on recommending better subscription alternatives
- Free subscription tracking (eat Plaid costs temporarily)
- Revenue from affiliate commissions (10-20% of subscription value)
- Example: Recommend switching from Netflix to Hulu → earn $2-3/month commission

#### Pivot 3: Niche Vertical
**Model**: Target specific high-value audience
- "Subscription tracker for freelancers" (business expense categorization)
- "Subscription tracker for families" (shared account optimization)
- Justifies higher pricing ($19.99/month) with specialized features

---

## Actionable Next Steps

### This Week
- [ ] **Pricing validation**: Survey 50 target users - would they pay $7.99/month?
- [ ] **Competitor testing**: Sign up for Rocket Money Premium - experience the concierge flow
- [ ] **Plaid sandbox**: Test recurring transactions API accuracy with real data
- [ ] **Legal consultation**: $500-1,000 for initial compliance assessment

### Next 4 Weeks
- [ ] **Financial model**: Build realistic 24-month P&L with corrected assumptions
- [ ] **Design mockups**: Figma prototype focusing on AI insights (not "one-tap")
- [ ] **Fundraising OR bootstrap decision**: Commit to Option A or Option B
- [ ] **Landing page**: Launch waitlist to gauge organic demand

### Next 3-6 Months (If Proceeding)
- [ ] **Build MVP**: Follow 20-week development roadmap
- [ ] **Beta test**: 50-100 early adopters with manual support
- [ ] **Measure metrics**: Conversion rate (target 15%+), churn, NPS
- [ ] **Iterate**: Kill features that don't drive conversion/retention

---

## Final Recommendation

### If You Have VC Access: BUILD (with revisions)
- The market is real and growing
- Plaid provides a strong technical foundation
- Concierge model works (Rocket Money proves it)
- **But**: Raise $500K minimum, target $7.99+ pricing, 18-24 month runway

### If You're Bootstrapping: VALIDATE FIRST
- Start with manual entry (no Plaid)
- Prove people will pay $7.99/month for just tracking + alerts
- **Then** add Plaid once you have $800+ MRR
- **Why**: Plaid costs will kill you before you validate the model

### If You Want True Passive Income: PIVOT
- SubScout requires customer support (concierge cancellations)
- Consider B2B2C or affiliate models instead
- Or accept that "passive" means "delegated support team" at scale

---

## Key Metrics to Watch

### Pre-Launch Validation
- **Waitlist conversion**: >10% of waitlist should convert to paid within 30 days
- **Pricing surveys**: >40% willing to pay $7.99/month
- **Manual MVP retention**: >60% of paying users stay past Month 2

### Post-Launch Success Criteria
- **Month 3**: 500 users, 12%+ paid conversion
- **Month 6**: 2,500 users, 15%+ paid conversion, <5% monthly churn
- **Month 12**: 10,000 users, $12K+ MRR, path to break-even visible

### Red Flags to Abandon
- <5% paid conversion after 6 months
- >10% monthly churn (users canceling Premium)
- Concierge cancellations cost >$15 per request (efficiency problem)
- Unable to scale beyond 5,000 users due to support bottleneck

---

## Resources & Documentation

1. **Full Technical Research**: See `SUBSCOUT_TECHNICAL_RESEARCH.md`
   - 50+ pages of detailed analysis
   - Plaid API documentation review
   - Compliance requirements breakdown
   - ML implementation strategies

2. **Recommended Reading**:
   - Plaid Docs: https://plaid.com/docs/transactions/
   - Rocket Money Teardown: https://www.cnbc.com/select/truebill-review/
   - FTC Click to Cancel: https://www.ftc.gov/news-events/news/press-releases/2024/10/

3. **Expert Consultations Needed**:
   - **Legal**: Fintech attorney ($300-500/hr) for compliance roadmap
   - **Financial**: CFO/advisor to validate revised projections
   - **Technical**: Senior fintech engineer to review Plaid integration plan

---

## Questions for Self-Reflection

Before proceeding, honestly answer:

1. **Capital**: Can I access $300-500K in funding? If no, can I bootstrap for 6-12 months without Plaid?

2. **Timeline**: Am I prepared for 18-24 months to break-even? Or do I need revenue in 6-12 months?

3. **Operational**: Am I excited about building a customer support team? Or did I want a fully automated product?

4. **Pricing**: Can I convince users to pay $7.99-14.99/month when Rocket Money charges $6-12? What's my differentiation?

5. **Competition**: Rocket Money has $100M+ in funding and 3.5M users. What's my unfair advantage?

---

## The Bottom Line

SubScout is **buildable** but **not as described in the original pitch**:

| Original Vision | Realistic Version |
|----------------|-------------------|
| Passive income with minimal support | Active support team for concierge service |
| One-tap automated cancellation | Human-assisted cancellation (like Rocket Money) |
| Free tier with basic features | Freemium difficult due to Plaid $500 minimum |
| $4.99/month pricing | $7.99-14.99/month needed for unit economics |
| Break-even at 1,000 users | Break-even at 15,000+ users |
| FTC rules create opportunity | FTC rule voided, never helped anyway |

**Build this if**: You can raise capital, pivot to concierge model, and compete on AI insights + UX
**Don't build this if**: You want true passive income, can't access funding, or expect automated cancellation

---

**Next Action Required**: Choose Option A (VC path), Option B (bootstrap), or Option C (pivot)

For questions or deeper analysis on specific areas, refer to the full technical research document.
