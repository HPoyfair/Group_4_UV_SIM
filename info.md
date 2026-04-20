# User Manual


## Getting Started

From the repository root, run the following commands:

```powershell
dotnet restore
```
then

```powershell
dotnet run --project .\UVGUI\UVGUI.csproj
```

## Simulator Editor



![App Screenshot](./images/Simulator-dash.png)

### File Operations

#### Load

Load a file by clicking the **Load** button. This will open a new tab with the selected file loaded into memory and will display in the GUI.

NOTE: This system supports 4 digit and 6 digit instructions, if you load a 6 digit file, the GUI will adapt automatically for that tab.

![App Screenshot](./images/6-digit-ex.png)

#### Save

You can save a file to your computer by clicking **Save** button. This will save a .txt file of your current tab (file).



### Run a program

To run a program, load a file into memory or edit the memory manually then click **Run**

#### User Input

Certain opcodes will write a value from the user into memory. Enter the desired value when prompted and press **OK**

![App Screenshot](./images/input-ex.png)


# Memory Editor

You can interact with each cell and input instructions into memory as well as use the **Toolbar** to perform actions on either one cell or multiple cells.

#### Select one cell 
![App Screenshot](./images/select-one.png)

#### Select multiple cells
![App Screenshot](./images/select-many.png)





# Toolbar 

The tool bar allows you to edit the file you are working on with the following commands
![App Screenshot](./images/toolbar.png)

#### Insert (depricated Action)
No longer has use.
#### Delete
Deletes instructions in a cell(s)
#### Copy
Copies instructions in a cell(s)
#### Cut
Copies and deletes instructions in a cell(s)
#### Paste
Pastes instructions at the selected cell
#### Close Tab
closes current selected tab
#### Theme
Configue theme with two colors
#### Reset Theme
Sets theme back to default theme








