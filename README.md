# This app was built for my portfolio and a learning experience.
# Recurring Budget Tracker


## Purpose

This application is designed to help you track and visualize your **recurring monthly expenses** to better understand your spending patterns and plan for financial decisions. This is **not** a typical expense tracking app where you log individual purchases or transactions.

## What This App Does

### 🏠 Track Fixed Monthly Expenses
- Mortgage/Rent payments
- Utility bills (electric, gas, water, internet)
- Insurance premiums (health, auto, home, life)
- Subscription services (Netflix, Spotify, gym memberships)
- Loan payments
- Other recurring financial obligations

### 📊 Visualize Spending Patterns
- See your expense-to-income ratio
- Understand where your money goes by category
- Identify your largest recurring expenses
- Track spending habits over time

### 💡 Plan Financial Decisions
- **Affordability Calculator**: Determine if you can afford new recurring expenses
- **Savings Potential**: See how much you have available for savings each month
- **Budget Analysis**: Understand if your spending ratio is healthy
- **Decision Support**: Make informed choices about new subscriptions or financial commitments

## Key Features

### Dashboard Overview
- Monthly income vs. recurring expenses comparison
- Available amount for savings and new expenses
- Expense ratio with health indicators
- Category-based spending breakdown
- Quick affordability assessments

### Expense Management
- Add/edit/remove recurring monthly expenses
- Categorize expenses for better visualization
- Track payment amounts and descriptions
- Monitor total monthly financial obligations

### Income Tracking
- Record all sources of regular monthly income
- Track salary, side income, investments, etc.
- Calculate total available monthly funds

### Smart Insights
- Expense ratio analysis (recommended: keep under 80% of income)
- Savings recommendations (suggest 20% of available funds)
- Spending pattern identification
- Financial health indicators

## What This App Is NOT

❌ **Individual Transaction Tracking**: This is not for logging every coffee purchase or grocery trip  
❌ **Variable Expense Monitoring**: Don't add expenses that change significantly month-to-month  
❌ **Daily Spending Diary**: This focuses on fixed, predictable monthly obligations  
❌ **Investment Portfolio Tracker**: This is for recurring expenses, not investment management  

## Who Should Use This App

✅ **Budget Planners**: People who want to understand their fixed monthly commitments  
✅ **Subscription Managers**: Those tracking multiple recurring services and memberships  
✅ **Financial Decision Makers**: Anyone considering new recurring expenses or financial commitments  
✅ **Savings Goals Planners**: People who want to maximize their monthly savings potential  
✅ **Debt Management**: Those tracking loan payments and working toward financial freedom  

## Example Use Cases

### Scenario 1: New Subscription Decision
*"I want to sign up for a $15/month streaming service. Can I afford it?"*
- Check your dashboard's "Available for Savings" amount
- See if you have room after recommended savings
- Make an informed decision based on your current commitments

### Scenario 2: Savings Goal Planning
*"How much should I be saving each month?"*
- View your total recurring expenses vs. income
- See the recommended 20% savings amount
- Identify areas where you might reduce recurring costs

### Scenario 3: Financial Health Check
*"Am I spending too much on fixed expenses?"*
- Review your expense ratio percentage
- Compare against the recommended 80% threshold
- Analyze spending by category to find optimization opportunities

## Getting Started

1. **Add Your Income Sources**: Start by recording all regular monthly income
2. **Add Recurring Expenses**: Include all fixed monthly payments and subscriptions
3. **Categorize Expenses**: Group similar expenses for better visualization
4. **Review Dashboard**: Analyze your spending patterns and available savings
5. **Make Informed Decisions**: Use the insights to guide financial choices

## Technology Stack

- **Backend**: ASP.NET Core MVC
- **Database**: Entity Framework Core
- **Frontend**: Bootstrap 5, HTML5, CSS3
- **Authentication**: ASP.NET Core Identity
- **Icons**: Font Awesome

## Project Structure

```
BudgetTracker/
├── BudgetTracker.Data/          # Database context and repositories
├── BudgetTracker.Models/        # Domain models and view models
├── BudgetTracker.Services/      # Business logic services
├── BudgetTracker.Web/           # MVC web application
└── scripts/                     # Development and deployment scripts
```

## Demo Mode

The application includes a comprehensive demo mode for showcasing the features:

### 🎯 Demo Features
- **Live Demo**: Visit `/demo` to access the demo information page
- **Auto-Login**: Automatically logs in with a demo user account
- **Sample Data**: Includes realistic income sources, expenses, and categories
- **Full Functionality**: Add, edit, and delete data to test all features
- **Auto-Reset**: Demo data automatically resets every 30 minutes
- **Manual Reset**: Use `/demo/reset` to manually reset demo data or click the Reset Demo button on Dashboard


## Contributing

This application focuses specifically on recurring expense tracking. When contributing, please maintain this focus and avoid features that would turn it into a general expense tracking or accounting application.

---

*Remember: This tool is about understanding your recurring financial commitments, not tracking every dollar you spend. Use it to make informed decisions about new subscriptions, set realistic savings goals, and maintain a healthy expense-to-income ratio.*
