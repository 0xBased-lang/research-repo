# SubScout Cost Calculator & Break-Even Analysis

Use this calculator to model different scenarios for SubScout's financial viability.

---

## Monthly Operating Cost Formula

```
TOTAL_MONTHLY_COST =
  (USERS × PLAID_RATE) +
  SUPABASE +
  STRIPE +
  HOSTING +
  COMMUNICATIONS +
  MONITORING +
  (CONCIERGE_REQUESTS × LABOR_COST)
```

---

## Variable Costs (Per-User)

### Plaid Pricing Tiers
| Users | Rate per User | Minimum Monthly |
|-------|--------------|-----------------|
| 0-334 | $1.50 | $0 (Pay-as-you-go) |
| 335-5,000 | $1.50 | $500 (Growth tier recommended) |
| 5,001-10,000 | $1.30 (negotiated) | $500 |
| 10,001-25,000 | $1.00 (negotiated) | $1,000 |
| 25,000+ | $0.70-0.80 (negotiated) | Custom |

### Concierge Labor Costs
| Scenario | Requests/Month | Minutes per Request | Hourly Rate | Monthly Cost |
|----------|----------------|---------------------|-------------|--------------|
| Low | 100 | 15 min | $15 | $375 |
| Medium | 500 | 20 min | $20 | $3,333 |
| High | 1,000 | 25 min | $25 | $10,417 |

**Formula**: `(Requests × Minutes / 60) × Hourly Rate`

---

## Fixed Costs (Monthly)

| Service | Tier | Cost | Notes |
|---------|------|------|-------|
| **Supabase** | Free | $0 | Up to 500MB database, 2GB bandwidth |
| | Pro | $25 | 8GB database, 50GB bandwidth |
| | Team | $150 | Production features, real-time |
| **Stripe** | Standard | $29 + 2.9% + $0.30/txn | Billing management |
| **AWS/Hosting** | Small | $50 | Background jobs, S3 storage |
| | Medium | $200 | Increased compute for 5K+ users |
| | Large | $500 | Enterprise scale (25K+ users) |
| **Email (Resend)** | Free | $0 | 3,000 emails/month |
| | Pro | $20 | 50,000 emails/month |
| | Business | $80 | 500,000 emails/month |
| **Push (OneSignal)** | Free | $0 | 10,000 users |
| | Growth | $30 | 50,000 users |
| **Monitoring (Sentry/DataDog)** | Small | $20 | Error tracking |
| | Medium | $50 | APM + logs |
| **Support/Admin** | Part-time | $1,500 | 20 hrs/week @ $18.75/hr |
| | Full-time | $4,000 | 40 hrs/week @ $25/hr |

---

## Revenue Formulas

### Monthly Recurring Revenue (MRR)
```
MRR = TOTAL_USERS × CONVERSION_RATE × PRICE_PER_MONTH

Example:
5,000 users × 10% conversion × $7.99 = $3,995 MRR
```

### Average Revenue Per User (ARPU)
```
ARPU = MRR / TOTAL_USERS

Example:
$3,995 MRR / 5,000 users = $0.799 ARPU
```

### Customer Lifetime Value (LTV)
```
LTV = PRICE_PER_MONTH × (1 / MONTHLY_CHURN_RATE)

Example (5% monthly churn):
$7.99 × (1 / 0.05) = $159.80 LTV
```

### Customer Acquisition Cost (CAC) Payback
```
PAYBACK_MONTHS = CAC / (PRICE_PER_MONTH × GROSS_MARGIN)

Example:
$30 CAC / ($7.99 × 0.60 gross margin) = 6.3 months
```

**Rule of Thumb**: LTV should be >3× CAC for healthy unit economics

---

## Scenario Modeling

### Scenario 1: Conservative (1,000 users, Month 6)

**Users**: 1,000
**Conversion Rate**: 8%
**Price**: $4.99/month

#### Revenue
```
MRR = 1,000 × 0.08 × $4.99 = $399
```

#### Costs
```
Plaid:           1,000 × $1.50        = $1,500
Supabase:        Pro tier             = $25
Stripe:          Base + (80 × $0.30)  = $53
Hosting:         Small                = $50
Email:           Pro                  = $20
Push:            Free                 = $0
Monitoring:      Small                = $20
Concierge:       80 requests × $7.50  = $600
                                      -------
TOTAL COSTS:                          = $2,268
```

#### Net
```
$399 - $2,268 = -$1,869/month LOSS ❌
```

---

### Scenario 2: Realistic (5,000 users, Month 12)

**Users**: 5,000
**Conversion Rate**: 12%
**Price**: $7.99/month

#### Revenue
```
MRR = 5,000 × 0.12 × $7.99 = $4,794
```

#### Costs
```
Plaid:           5,000 × $1.30         = $6,500
Supabase:        Team tier             = $150
Stripe:          Base + (600 × $0.30)  = $209
Hosting:         Medium                = $200
Email:           Business              = $80
Push:            Free                  = $0
Monitoring:      Medium                = $50
Concierge:       600 requests × $8     = $4,800
                                       -------
TOTAL COSTS:                           = $11,989
```

#### Net
```
$4,794 - $11,989 = -$7,195/month LOSS ❌
```

---

### Scenario 3: Optimistic (15,000 users, Month 18)

**Users**: 15,000
**Conversion Rate**: 15%
**Price**: $9.99/month

#### Revenue
```
MRR = 15,000 × 0.15 × $9.99 = $22,478
```

#### Costs
```
Plaid:           15,000 × $1.00         = $15,000
Supabase:        Team tier              = $150
Stripe:          Base + (2,250 × $0.30) = $704
Hosting:         Large                  = $500
Email:           Business               = $80
Push:            Growth                 = $30
Monitoring:      Medium                 = $50
Concierge:       1,800 requests × $8    = $14,400
                                        -------
TOTAL COSTS:                            = $30,914
```

#### Net
```
$22,478 - $30,914 = -$8,436/month LOSS ❌
```

**Issue**: Even at 15K users, concierge costs are killing profitability!

---

### Scenario 4: Profitable Model (15,000 users, Limited Concierge)

**Users**: 15,000
**Conversion Rate**: 15%
**Price**: $9.99/month
**Concierge Limit**: 2 per user per year (vs. unlimited)

#### Revenue
```
MRR = 15,000 × 0.15 × $9.99 = $22,478
```

#### Costs
```
Plaid:           15,000 × $1.00        = $15,000
Supabase:        Team tier             = $150
Stripe:          Base + (2,250 × $0.30)= $704
Hosting:         Large                 = $500
Email:           Business              = $80
Push:            Growth                = $30
Monitoring:      Medium                = $50
Concierge:       375 requests × $8     = $3,000
                                       -------
TOTAL COSTS:                           = $19,514
```

#### Net
```
$22,478 - $19,514 = $2,964/month PROFIT ✅
```

**Gross Margin**: 13.2% (low but viable if CAC is controlled)

---

## Break-Even Calculator

### Formula
```
BREAK_EVEN_USERS = FIXED_COSTS / (REVENUE_PER_USER - VARIABLE_COST_PER_USER)

Where:
REVENUE_PER_USER = PRICE × CONVERSION_RATE
VARIABLE_COST_PER_USER = PLAID_RATE + (CONCIERGE_RATE × CONVERSION_RATE)
FIXED_COSTS = Supabase + Stripe + Hosting + etc.
```

### Example Calculation

**Assumptions**:
- Price: $9.99/month
- Conversion: 15%
- Plaid: $1.00/user
- Concierge: $8/request, 2 requests per user per year = $1.33/month
- Fixed costs: $2,500/month

```
REVENUE_PER_USER = $9.99 × 0.15 = $1.50
VARIABLE_COST_PER_USER = $1.00 + ($1.33 × 0.15) = $1.20
CONTRIBUTION_MARGIN = $1.50 - $1.20 = $0.30

BREAK_EVEN_USERS = $2,500 / $0.30 = 8,334 users
```

**Insight**: You need 8,334+ users to break even at these assumptions.

---

## Sensitivity Analysis

### Impact of Price Changes

| Price | Conv Rate | Users Needed | Notes |
|-------|-----------|--------------|-------|
| $4.99 | 10% | **25,000+** | Original plan - very hard to achieve |
| $6.99 | 12% | 15,000 | Better but still challenging |
| $7.99 | 12% | 12,000 | Competitive with market |
| $9.99 | 15% | **8,000** | Optimal for profitability |
| $14.99 | 18% | 6,000 | Premium positioning |

**Key Insight**: Higher pricing REDUCES users needed for break-even (if conversion holds)

### Impact of Conversion Rate Changes

| Conv Rate | Price | Users Needed | Likelihood |
|-----------|-------|--------------|------------|
| 5% | $9.99 | 50,000+ | Unrealistic scale |
| 8% | $9.99 | 20,000 | Still very difficult |
| 12% | $9.99 | 10,000 | Achievable with strong product |
| 15% | $9.99 | **8,000** | Best-in-class conversion |
| 20% | $9.99 | 5,000 | Exceptional (unlikely) |

**Benchmark**: SaaS average is 2-5% free-to-paid conversion. 12-15% is top quartile.

### Impact of Plaid Negotiations

| Users | Standard Rate | Negotiated Rate | Monthly Savings |
|-------|---------------|-----------------|----------------|
| 5,000 | $1.50 | $1.30 | $1,000 |
| 10,000 | $1.50 | $1.00 | $5,000 |
| 25,000 | $1.50 | $0.70 | $20,000 |

**Strategy**: Don't integrate Plaid until you can negotiate. Bootstrap with manual entry first.

---

## MVP Cost Breakdown

### One-Time Development Costs

| Category | Low | Medium | High |
|----------|-----|--------|------|
| Design | $3,000 | $5,000 | $8,000 |
| Frontend (React Native) | $15,000 | $25,000 | $35,000 |
| Backend (Supabase) | $8,000 | $12,000 | $18,000 |
| Plaid Integration | $3,000 | $4,500 | $6,000 |
| ML/Analytics | $5,000 | $8,000 | $12,000 |
| QA/Testing | $2,000 | $3,500 | $5,000 |
| Project Management | $3,000 | $4,500 | $6,000 |
| Legal/Compliance | $5,000 | $8,000 | $12,000 |
| **TOTAL** | **$44,000** | **$70,500** | **$102,000** |

### 24-Month Runway (Operating Costs)

| Milestone | Users | Monthly Cost | Months | Total |
|-----------|-------|--------------|--------|-------|
| **Pre-Launch** | 0 | $500 | 5 | $2,500 |
| **Month 1-6** | 100-1,000 | $2,000 | 6 | $12,000 |
| **Month 7-12** | 1,000-5,000 | $8,000 | 6 | $48,000 |
| **Month 13-18** | 5,000-12,000 | $18,000 | 6 | $108,000 |
| **Month 19-24** | 12,000-20,000 | $25,000 | 6 | $150,000 |
| **TOTAL OPERATING** | | | | **$320,500** |

### Total Capital Required (24 Months to Profitability)

```
MVP Development:      $70,500
Operating (24 mo):    $320,500
Marketing/CAC:        $50,000
Buffer (20%):         $88,200
                      --------
TOTAL FUNDING NEEDED: $529,200
```

**Recommendation**: Raise $500-600K seed round for 24-month runway.

---

## Quick Decision Matrix

### Should You Build SubScout? (Yes/No Checklist)

| Criteria | Status | Weight | Score |
|----------|--------|--------|-------|
| Can access $300K+ funding? | ☐ Yes ☐ No | 10 | /10 |
| Comfortable with 18-24 month timeline? | ☐ Yes ☐ No | 9 | /9 |
| Willing to charge $7.99+ pricing? | ☐ Yes ☐ No | 8 | /8 |
| Can achieve 12%+ conversion rate? | ☐ Yes ☐ No | 8 | /8 |
| Okay with concierge model (not automated)? | ☐ Yes ☐ No | 7 | /7 |
| Have fintech/SaaS experience? | ☐ Yes ☐ No | 6 | /6 |
| Can build customer support team? | ☐ Yes ☐ No | 6 | /6 |
| Differentiation beyond Rocket Money? | ☐ Yes ☐ No | 5 | /5 |
| **TOTAL** | | **59** | **/59** |

**Scoring**:
- **45+ points**: Strong candidate - proceed to MVP
- **30-44 points**: Conditional - bootstrap first to validate
- **<30 points**: High risk - consider pivot

---

## Alternative Business Models

### Model 1: B2B2C White-Label

**Revenue**: $5-15K per bank partner per month
**Users**: Bank's existing customer base (no Plaid costs)
**Break-even**: 5-10 bank partners

**Pros**:
- Higher revenue per customer
- No Plaid costs
- Enterprise contracts (stable)

**Cons**:
- Long sales cycles (6-12 months)
- Customization work per partner
- Less control over end-user experience

### Model 2: Affiliate-First

**Revenue**: 10-20% commission on recommended subscriptions
**Example**: User cancels Netflix ($15.99) and switches to Hulu ($7.99 via affiliate link) = $1.60 commission/month

**Break-even**: Depends on churn of recommended subscriptions

**Pros**:
- Can offer free tier (eat Plaid costs)
- Aligned with user savings goals
- Scalable once recommendation engine works

**Cons**:
- Revenue per user is low ($1-3/month)
- Dependent on affiliate programs
- Race to bottom with cashback apps

### Model 3: Freemium with Ads

**Revenue**: Ad impressions + Premium subscriptions
**CPM**: $2-5 for fintech audience

**Example**:
- 10,000 free users × 20 ad views/month × $3 CPM = $600 ad revenue
- 1,500 paid users × $7.99 = $11,985 subscription revenue
- **Total**: $12,585 MRR

**Pros**:
- Monetize free users
- Higher total revenue

**Cons**:
- Degrades user experience
- Low CPM in finance category
- Conflicts with "savings" mission

---

## Your Custom Calculation

Fill in your assumptions:

```
USERS: _______
CONVERSION_RATE: ______%
PRICE_PER_MONTH: $_____
PLAID_RATE: $_____
CONCIERGE_REQUESTS_PER_PAID_USER: _____
LABOR_COST_PER_REQUEST: $_____

REVENUE = Users × Conversion × Price
        = _____ × _____ × _____
        = $_____

COSTS = (Users × Plaid_Rate) + Fixed_Costs + Concierge_Costs
      = (_____ × _____) + _____ + (_____ × _____)
      = $_____

NET = Revenue - Costs
    = _____ - _____
    = $_____ (profit/loss)
```

---

## Key Takeaways

1. **Plaid is expensive**: $1.50/user eats up almost all revenue at small scale
2. **Concierge costs scale with users**: Unlike SaaS, your costs grow with usage
3. **Break-even is far away**: Expect 15,000+ users before profitability
4. **High pricing helps**: $9.99/month works better than $4.99/month
5. **Conversion is critical**: 12-15% needed vs. industry average of 2-5%
6. **Capital intensive**: Need $300-500K to reach profitability

**Bottom Line**: This is a venture-backable business, NOT a bootstrap/passive income opportunity.
