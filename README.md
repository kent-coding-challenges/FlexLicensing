# Flex Licensing
A coding challenge to calculate minimum number of licenses required given a software licensing rule which restricts the maximum installation allowed on different devices.

## Problem Description
Some vendors allow a user to reuse the same software license across different computers with specific restrictions. In this challenge, we have an application with ID 374 (let's say this is **MS Office**), where user can use one license to be installed in two computers if at least one of them is a laptop.

Let's create a class lirbary which provides functionality to calculate minimum number of license a company must purchase given a CSV input file. The input file has the following headers: ComputerID, UserID, ApplicationID, ComputerType, Comment. Furthermore, the CSV can contain duplicate records (which should be ignored), comments are not to be considered in pulling the records, and casing for ComputerType is case-insensitive.

### Sample Input / Expected Output
| ComputerID    | UserID        | ApplicationID  | ComputerType  | Comment                     |     
| ------------- |:------------- | -------------- |:------------- |:--------------------------- |
| 1             | 1             | 374            | DESKTOP       | Exported from ....          |
| 2             | 1             | 374            | DESKTOP       | Exported from ....          |
| 2             | 1             | 374            | desktop       | Exported from ....          |
| 3             | 2             | 374            | LAPTOP        | Exported from ....          |
| 4             | 2             | 374            | DESKTOP       | Exported from ....          |

In the sample input above, row #2 and #3 are duplicates. Hence, User #1 requires 2 license copies and User #2 requires 1 license copy. In this case, the company needs to purchase minimum of 3 licenses for its users.

### Input Size Analysis
Input size can be very large, as we are given these two sample files:
1. **sample-small.csv**

Approximately 220'000 records (10mb)

2. **sample-large.csv**

Approximately 22'000'000 records (1gb)

### Allowed Assumptions
Unexpected situations won't have to be considered. This may include empty input values, computers with multiple users or computers that are both dekstop and laptop.

## Solution
### Input Reading Strategy
To be added.

### License Rules Modelling
To support dynamic licensing model, we are going to model License Rule in the following way:
```c#
int TotalMaxInstall;
Dictionary<ComputerType, uint> MaxInstallPerComputerType;
```

For instance, license rule for MS Office can be modelled in the following way:
```
TotalMaxInstall: 2,
MaxInstallPerComputerType: {
  Desktop: 1,
  Laptop: 2
}
```

### Mathematical Model Attempt
The following equation is an attempt to generalize this problem into a mathemetical model.

![FlexLicensing Initial Mathematical Model - kent](http://i.imgur.com/mSBKDrg.png)

However, this mathematical model hasn't been fully worked out yet for the following scenarios:
1. Different Ytotal instead of limiting Ytotal = Ymin + Ymax.
2. When more ComputerTypes are considered (not only Desktop and Laptop).

While I believe that the optimal solution lies in working out the above equation further to cover its current limitations, this mathematical model is temporarily dropped until it can be worked out further to remove its current limitations.

### Current Implementation
This calculation module can be found in *GetMinLicenseRequired()* function in *LicenseCalculator.cs*.

#### Here's the high-level view of how it's implemented:
```
SET license = 0
SET userLogs = logs.GroupBy(userID)
FOREACH userLog in userLogs:
  GET summarized Dictionary<ComputerType, count> for current user
  SET licenseForUser = 0
  INCREMENT licenseForUser and consecutively apply this new license to computers belonging to users (using greedy approach).
  REPEAT previous step until:
    Current license exhausted (MaxInstall reached or no more computer can be licensed),
    OR All computers have been licensed (completion)
  license += licenseForUser
  
RETURN license
```

#### Complexity of Current Implementation
The complexity of current implementation is **O(UCLR)**.

where:
```
U: number of users
C: number of computer types
L: number of min licenses (output)
R: number of TotalMaxInstall specified in LicenseRule
```

However, as C and R tends to be very small, we can consider these two variables as constant values <= 20. With this assumption, the complexity of current implementation can be rewritten as:

Simplified Complexity: **O(UL)**
