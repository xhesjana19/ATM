# ATM Algorithm

This document describes the functionality and rules implemented in the ATM (Automated Teller Machine) program. The ATMs are configured to operate under the following conditions:

## Rules and Configurations

1. **Minimum Withdrawal Amount**  
   The minimum amount a customer can withdraw is **500 ALL**.

2. **Maximum Daily Withdrawal Amount**  
   The maximum daily withdrawal limit is configurable per customer. For example:
   - 30,000 ALL
   - 50,000 ALL

3. **Available Banknote Denominations**  
   The ATM dispenses banknotes in the following denominations:
   - 500 ALL
   - 1,000 ALL
   - 2,000 ALL
   - 5,000 ALL

4. **Daily Replenishment**  
   The ATM is replenished daily with a predefined quantity of banknotes.

5. **Withdrawal Amount Requirements**  
   The withdrawal amount must be a multiple of **500 ALL**. For example:
   - 500 ALL
   - 1,000 ALL
   - 1,500 ALL
   - ...
   - 45,000 ALL
   - 50,000 ALL

6. **Database Requirements**  
   The database must:
   - Log every withdrawal transaction.
   - Record the quantities of banknotes dispensed for each withdrawal.
   - Store the **optimal distribution** of banknotes for the withdrawal, even if the actual distribution differs.

## Notes
- The program ensures efficient handling of ATM operations while maintaining accurate records of transactions and banknote availability.
- Flexibility in configurations allows the system to cater to different customer needs and transaction patterns.
