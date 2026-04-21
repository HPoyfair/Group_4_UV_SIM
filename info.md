# Unit Tests

### Testing Overview
- AddInstructionTests: 2
- ArithmeticTests: 15
- ConsoleTests: 6
- ControlTests: 5
- LoadStoreTests: 10
- TabsStateTests: 3
- WriteInstructionTests: 2

- **Total: 43**

### AddInstructionTests Unit Tests

Overview:
This test file verifies the behavior of the ADD instruction, ensuring correct arithmetic execution and proper handling of invalid memory addresses.

Test Cases:
- Add_ValidOperand_AddsCorrectly – Verifies that the ADD instruction correctly adds the value from a valid memory address to the accumulator and increments the instruction pointer.
- Add_InvalidOperand_DoesNotThrow_AndAdvancesInstructionPointer – Ensures that using an invalid memory address does not throw an exception, does not modify the accumulator, and still advances the instruction pointer.


### ArithmeticTests Unit Tests

Overview:
This test file verifies all arithmetic operations (ADD, SUBTRACT, MULTIPLY, DIVIDE) across both Legacy (4-digit) and Extended (6-digit) formats, including correct calculations, invalid address handling, and overflow behavior.

Test Cases:
- Add_ValidOperand_AddsCorrectly_Legacy – Verifies ADD correctly adds a value from a valid memory address in legacy format and advances the instruction pointer.
- Add_InvalidOperand_DoesNotChangeAccumulator_Legacy – Ensures invalid address does not modify the accumulator in legacy format and still advances the instruction pointer.
- Add_ValidExtendedAddress_AddsCorrectly_Extended – Verifies ADD works correctly with valid extended memory addresses.
- Add_InvalidOperand_DoesNotChangeAccumulator_Extended – Ensures invalid address does not affect the accumulator in extended format.
- Add_LegacyOverflow_DoesNotChangeAccumulator – Confirms that overflow in legacy format does not update the accumulator.
- Add_ExtendedOverflow_DoesNotChangeAccumulator – Confirms that overflow in extended format does not update the accumulator.

- Subtract_Valid_SubtractsMemoryFromAccumulator – Verifies SUBTRACT correctly subtracts a memory value from the accumulator.
- Subtract_InvalidOperand_DoesNotChangeAccumulator – Ensures invalid address does not modify the accumulator for SUBTRACT.

- Multiply_ValidOperand_MultipliesAccumulator – Verifies MULTIPLY correctly multiplies the accumulator by a memory value.
- Multiply_ByZero_SetsAccumulatorToZero – Ensures multiplying by zero correctly sets the accumulator to zero.
- Multiply_LegacyOverflow_DoesNotChangeAccumulator – Confirms overflow in legacy format does not update the accumulator.
- Multiply_ExtendedOverflow_DoesNotChangeAccumulator – Confirms overflow in extended format does not update the accumulator.

- Divide_ValidOperand_DividesAccumulator – Verifies DIVIDE correctly divides the accumulator by a memory value.
- Divide_ByZero_DoesNotChangeAccumulator – Ensures division by zero does not modify the accumulator.
- Divide_InvalidOperand_DoesNotChangeAccumulator – Ensures invalid address does not modify the accumulator during division.


### ConsoleTests Unit Tests

Overview:
This test file verifies console-related instruction behavior, including input, output, invalid address handling, and proper instruction pointer updates in both Legacy (4-digit) and Extended (6-digit) formats.

Test Cases:
- Subtract_InvalidAddress_DoesNotThrow_AndAdvancesInstructionPointer – Verifies that SUBTRACT with an invalid memory address does not throw an exception, does not modify the accumulator, and still advances the instruction pointer.
- Input_ReadsValueIntoMemory – Verifies that READ stores the requested input value into the correct legacy memory address and advances the instruction pointer.
- Read_StoresFirstInputValue_AndAdvancesInstructionPointer_Extended – Verifies that READ stores a valid extended-format input value in memory, advances the instruction pointer, and only requests input once.
- Read_LegacyOutOfRangeInput_DoesNotStoreValue – Ensures that an out-of-range input value for legacy format is rejected and not stored in memory, while still advancing the instruction pointer.
- Write_OutputsMemoryValue_AndAdvancesIP_Legacy – Verifies that WRITE outputs the correct memory value in legacy format and advances the instruction pointer.
- Write_OutputsExtendedFormattedValue_AndAdvancesIP – Verifies that WRITE outputs the correct memory value in extended format and advances the instruction pointer.


### ControlTests Unit Tests

Overview:
This test file verifies control flow instructions, including unconditional branching and conditional branching based on the accumulator value.

Test Cases:
- Branch_Always_JumpsToOperand – Verifies that the BRANCH instruction always sets the instruction pointer to the operand address.
- BranchZero_WhenAccumulatorZero_Jumps – Ensures that BRANCHZERO jumps to the operand address when the accumulator is zero.
- BranchZero_WhenAccumulatorNotZero_Advances – Verifies that BRANCHZERO advances the instruction pointer normally when the accumulator is not zero.
- BranchNeg_WhenAccumulatorNegative_Jumps – Ensures that BRANCHNEG jumps to the operand address when the accumulator is negative.
- BranchNeg_WhenAccumulatorPositive_Advances – Verifies that BRANCHNEG advances the instruction pointer when the accumulator is positive.


### LoadStoreTests Unit Tests

Overview:
This test file verifies the LOAD and STORE instructions in both Legacy (4-digit) and Extended (6-digit) formats, including valid memory access, invalid address handling, and correct instruction pointer updates.

Test Cases:
- STORE_ValidMemory_UpdatesMemory – Verifies that STORE writes the accumulator value into a valid legacy memory address and advances the instruction pointer.
- STORE_InvalidMemoryAddress_DoesNotStore_Legacy – Ensures that STORE does not modify memory when given an invalid legacy memory address and still advances the instruction pointer.
- STORE_NegativeMemoryAddress_DoesNotStore – Verifies that STORE does not write to memory when given a negative address and still advances the instruction pointer.
- STORE_ValidExtendedMemory_UpdatesMemory – Verifies that STORE correctly writes the accumulator value to a valid extended memory address.
- STORE_InvalidMemoryAddress_DoesNotStore_Extended – Ensures that STORE does not modify memory when given an invalid extended memory address.

- LOAD_ValidMemory_UpdatesAccumulator – Verifies that LOAD copies a value from a valid legacy memory address into the accumulator and advances the instruction pointer.
- LOAD_InvalidMemoryAddress_DoesNotChangeAccumulator_Legacy – Ensures that LOAD does not modify the accumulator when given an invalid legacy memory address.
- LOAD_NegativeMemoryAddress_DoesNotChangeAccumulator – Verifies that LOAD does not change the accumulator when given a negative memory address.
- LOAD_ValidExtendedMemory_UpdatesAccumulator – Verifies that LOAD correctly copies a value from a valid extended memory address into the accumulator.
- LOAD_InvalidMemoryAddress_DoesNotChangeAccumulator_Extended – Ensures that LOAD does not modify the accumulator when given an invalid extended memory address.


### TabsStateTests Unit Tests

Overview:
This test file verifies that multiple CPU instances (used for separate tabs) maintain independent state, including memory, registers, and output logs.

Test Cases:
- SeparateCpuStates_MaintainIndependentMemory – Verifies that different CPU instances store and maintain separate memory values without interfering with each other.
- SeparateCpuStates_MaintainIndependentRegisters – Ensures that accumulator and instruction pointer values are independent between different CPU instances.
- SeparateOutputLogs_RemainIndependent – Verifies that output logs for different contexts remain separate and do not mix values between instances.


### WriteInstructionTests Unit Tests

Overview:
This test file verifies the WRITE instruction, including correct output formatting and proper handling of invalid memory addresses.

Test Cases:
- Write_Valid_UsesCurrentOutputFormat – Verifies that WRITE outputs the correct formatted value from memory using the current format and advances the instruction pointer.
- Write_InvalidAddress_PrintsCurrentErrorMessage – Ensures that WRITE prints an appropriate error message when given an invalid memory address and still advances the instruction pointer.