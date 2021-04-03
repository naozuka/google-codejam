That is a tricky problem since you can sort one by one but you have a limited number of questions for the judge. 
The solution is to always divide your array in three parts and go in until you find the exact position of your number.

```
    0   1   2   3   4   5   6   7   8   9  
    |           |           |           |
        First       Middle       Last    
```

Example of first test case with local test tool.

You can start your array with [1, 2] in your array.

```
Insert: 3 => [1 2]
   Asks for: "3 1 2", gets: 1
Result: [3 1 2]
```
```
Insert: 4 => [3 1 2]
   Asks for: "4 3 1", gets: 4
Result: [3 4 1 2]
```
```
Insert: 5 => [3 4 1 2]
   Asks for: "5 4 1", gets: 5
Result: [3 4 5 1 2]
```
```
Insert: 6 => [3 4 5 1 2]
   Asks for: "6 4 1", gets: 6
   Asks for: "6 4 5", gets: 6
Result: [3 4 6 5 1 2]
```
```
Insert: 7 => [3 4 6 5 1 2]
   Asks for: "7 4 5", gets: 7
   Asks for: "7 4 6", gets: 6
   Asks for: "7 6 5", gets: 7
Result: [3 4 6 7 5 1 2]
```
```
Insert: 8 => [3 4 6 7 5 1 2]
   Asks for: "8 4 5", gets: 5
   Asks for: "8 5 1", gets: 1
   Asks for: "8 1 2", gets: 2
Result: [3 4 6 7 5 1 2 8]
```
```
Insert: 9 => [3 4 6 7 5 1 2 8]
   Asks for: "9 6 1", gets: 6
   Asks for: "9 3 4", gets: 9
Result: [3 9 4 6 7 5 1 2 8]
```
```
Insert: 10 => [3 9 4 6 7 5 1 2 8]
   Asks for: "10 4 5", gets: 5
   Asks for: "10 1 2", gets: 1
   Asks for: "10 5 1", gets: 10
Result: [3 9 4 6 7 5 10 1 2 8]
```
