# UVSim – BasicML Simulator

## Overview

UVSim is a simple virtual machine written in C#.  
It simulates a BasicML computer with 100-word memory and a simulated CPU that stores memory (0-99), accumulator, instruction pointer, and a halt flag.
The user picks a test file (.txt) of instructions for the machine and the program executes each instruction as it iterates through the instructions in the simulated memory.

## Design

# Operations
Each cs file here is a category of operations with BasicML opcodes are divided into 4 sections in the operations directory within Group_4_UV_SIM.
- Arithmetic: ADD, SUB, MUL, DIV
- LoadStore: LOAD, STORE
- InputOutput: READ, WRITE
- Control: BRANCH, BRANCHNEG, BRANCHZERO, HALT

# Simulator.cs
The simulator contains the main runtime loop. It reads the txt file into memory and uses the  ExectuteInstructions method that correctly determines which instruction to run on each instruction from memory using a switch statement
- ReadFile method: loads every instructions line by line to the memory array as integers.
- ExecuteInstructions method: contains switch statement with cases of opcodes to trigger each operations method (ex: Control.Branch(opcode, operand, CPUstate))
- Run: gets file path from user, then iterates over memory to use ExecuteInstructions on each index.

# Program.cs
Main: runs Simulator.run()
---


## Requirements

- .NET 8.x.x.x SDK installed


---

## Running the program
from the terminal
- dotnet run
- enter Test1.txt 

## Run Tests
from the terminal 
- dotnet test



