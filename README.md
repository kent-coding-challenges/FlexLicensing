# FlexLicensing
A coding challenge to calculate minimum number of licenses required given a dynamic software licensing rule which restricts the maximum installation allowed on different devices.

## Problem Description
Application license can be reused across different computers per user with specific restrictions. For insstance, each copy of application with ID 374 can be installed in two computers if at least one of them is a laptop. For documentation purpose, let's say AppID 374 is MS Office.

The challenge is to create a class library which can calculate the minimum number of licenses a company must purchase to use MS Office.

The program takes a CSV input file with the following headers: ComputerID, UserID, ApplicationID, ComputerType, Comment. The input data can contain duplicate records (which should be ignored), and casing for ComputerType is case-insensitive.

### Sample Input / Expected Output
| ComputerID    | UserID        | ApplicationID  | ComputerType  | Comment                     |     
| ------------- |:-------------:| --------------:|:-------------:| ---------------------------:|
| 1             | 1             | 374            | DESKTOP       | Exported from ....          |
| 2             | 1             | 374            | DESKTOP       | Exported from ....          |
| 2             | 1             | 374            | desktop       | Exported from ....          |
| 3             | 2             | 374            | LAPTOP        | Exported from ....          |
| 4             | 2             | 374            | DESKTOP       | Exported from ....          |

Notice that row #2 and #3 with ComputerID = 2 are duplicates. Using the rule mentioned above for MS Office, User 1 requires 2 licenses and user 2 requires 1 license. Hence, the company needs to purchase a minimum of 2 MS Office licenses.

### Input Size
We are given two sample input csv. One with ~220'000 records (~10mb) called *sample-small.csv* and another with 22'000'000 records (~1gb) called *sample-large.csv*.

### Input Analysis
1. *Comment* column is not required for calculation purpose and can be ignored, considering input file size can be very large.
2. Ideally, we should know all possible ComputerType beforehand. Hence, we can map this column as an Enum value.

### Assumptions
Unexpected situations won't have to be considered, such as empty input values, computers with multiple users or computers that are both dekstop and laptop.

## Solution
### Input Reading Strategy
To be added.

### Problem-Specific Mathematical Model
The model discussed in this section only works specifically for MS Office licensing rule, as stated above.

Given two set of values, number of desktops and number of laptops, N1 and N2 with the following model:
```
TotalMaxInstall: 2
MaxInstallPerComputerType: {
  Desktop: 1,
  Laptop: 2
}
```

Number of min licenses required (Lc) can be modelled as below:
// Lc = Min(N1, N2) + Math.Ceil(Max(N1, N2) / NRi)
// Formula needs testing and more documentation, will update this soon.

### Data Modelling
To be added.

### Min License Calculation
This calculation module can be found in *GetMinLicenseRequired()* function in *LicenseCalculator.cs*.

Here's the general idea behinds this code:
```
SET license = 0
SET userLogs = logs.GroupBy(userID)
FOREACH userLog in userLogs {
  GET summarized Dictionary<ComputerType, count> for current user
  SET licenseForUser = 0
  INCREMENT licenseForUser and consecutively apply this new license to computers belonging to users (using greedy approach).
  REPEAT previous step until:
    MaxInstall for license is reached, all computers are licensed, or no more computer can be applied with license.
  INCREMENT license by licenseForUser
}
RETURN license
```
