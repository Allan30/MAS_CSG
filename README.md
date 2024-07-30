# Multi-Agent System (MAS) project

  * [Presentation](#presentation)
  * [Selected architecture](#selected-architecture)
  * [Implemented features](#implemented-features)
  * [Demonstration](#demonstration)
  * [Missing Features](#missing-features)

## Presentation
The goal of this project is to create a community travel system similar to Blablacar (french carpooling). We have a set of travelers, each with a departure city and a destination city, and a set of vehicles. The objective is to optimize the number of occupied vehicles so that all travelers reach their destinations, adhering to the departure and arrival dates as well as the desired prices. This is a coalition structure generation (CSG) problem.

We have chosen to develop the project using Microsoft's open-source C# language (version 12 of the language and version 8 of .NET). The architecture and syntax are therefore quite similar to what one might find in Java.

## Selected architecture
![image](https://github.com/user-attachments/assets/aedef890-0e51-4371-9fa8-7f165657ae26)

## Implemented features
- **Function for Calculating Coalition Values  → CalculateCoalitionValue**:
  
  To calculate the values of the coalitions and avoid assigning them a random value, we decided to create a calculation function based on several parameters:


  - **Departure and arrival cities**: We add 1,000,000 points for each agent who does not have the same departure city, and similarly for the arrival city.
  - **Departure date**: We add 1 point for each minute of deviation from the closest departure date.
  - **Car price**: We add the car price in points, divided by the number of agents.
  - **Maximum penalty**: If there are not enough cars compared to the number of groups in the coalition, we return the maximum value of the double type.
  
  The goal is to minimize the number of points.

- **Dynamic Programming Algorithm (DP) → DPAlgorithm.cs**
- **Improved Dynamic Programming Algorithm (IDP) → IDPAlgorithm.cs**

## Demonstration
In our demonstration we have 9 agents such as:

| Name  | Start city  | End city | Depart date |
| :---- |:----------- |:-------- |:----------- |
| agent1 | Paris | Lyon | 12/25/2024 10:40:00 | 
| agent2 | Paris | Lyon | 12/25/2024 20:30:00 | 
| agent3 | Paris | Lyon | 12/25/2024 10:30:00 | 
| agent4 | Paris | Lyon | 12/25/2024 20:50:00 | 
| agent5 | Lyon | Paris | 12/26/2024 10:30:00 | 
| agent6 | Lyon | Paris | 12/26/2024 10:30:00 | 
| agent7 | Marseille | Lyon | 12/25/2024 10:40:00 | 
| agent8 | Paris | Lyon | 12/25/2024 10:20:00 | 
| agent9 | Paris | Lyon | 12/25/2024 10:40:00 | 

and 3 car types:
| Name  | Price | Capacity| 
| :---- |:----------- |:-------- |
| car1 | 90 | 4 |
| car2 | 80 | 3 |
| car3 | 200 | 5 |

We know that both algorithms will yield the same result (an optimal result), and it is apparent that they will group agents traveling Lyon→Paris and Paris→Lyon together, forming either groups of 4 or 2 based on the departure times (10:30 and 20:30), with a single car for the person traveling Marseille→Paris. They will distribute into 4 cars according to the color coding used in the table. By watching the video, we can confirm that the same result is obtained for both algorithms.

## Missing Features
- Effective Dynamic Programming (EDP) Algorithm
- Consideration of Certain Preferences in Addition to Standard Constraints: number of passengers, smoker or non-smoker, talkative or not…
