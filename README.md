

# Txt2Rabbit

This project provides two implementations for converting rally road book data into a format compatible with Rabbit:

- **Python Console Application** (in `rabbit-python/`)
- **.NET MAUI Hybrid Application** (in `rabbit-app/rabbit-maui/`)

Both tools help automate the process of transforming simple stage lists into the Excel format expected by Rabbit for import.


## Use Case

You start with a road book containing only the stages, each with segment distance and ideal time (in minutes). The goal is to convert this info in the physical book into a Rabbit-compatible XLSX file, which can then be imported into the Rabbit application for further rally management.

**Workflow:**
1. If using the Python console app: Prepare a TXT file where each line contains a segment's distance and ideal time, separated by space or tab. Blank lines separate stages.
2. If using the Python console app: Run the script to convert this TXT into an XLSX file.
3. If using the MAUI hybrid app: Open the app and enter your stages and segments (distance and ideal time) directly in the UI, then export the XLSX file.
4. Import the generated XLSX into Rabbit.

This process saves time and reduces errors when preparing rally data for Rabbit.

---

## Project Structure

- `rabbit-python/`: Python console application for TXT to Rabbit XLSX conversion
- `rabbit-app/rabbit-maui/`: .NET MAUI hybrid application with a graphical interface for the same conversion

---


## Python Console Application (`rabbit-python/`)

This script reads a structured TXT file and outputs an Excel (.xlsx) file formatted for Rabbit import.

**Features:**
- Reads structured text files and outputs Excel (.xlsx) files
- Simple command-line interface
- Customizable for different text formats
- Generates labels for each section (e.g., E1-T1)
- Supports multiple stages separated by blank lines

**Requirements:**
- [openpyxl](https://pypi.org/project/openpyxl/) (for Excel file creation)
- [pandas](https://pypi.org/project/pandas/) (for data manipulation)
- [xlsxwriter](https://pypi.org/project/XlsxWriter/) (for Excel writing engine)


### Installation
1. Navigate to `rabbit-python/`:
   ```sh
   cd rabbit-python
   ```
2. Install dependencies:
   ```sh
   pip install -r requirements.txt
   ```


### Input Format
The input TXT file should contain distances and times for rally stages, with blank lines separating each stage. Each line should have two numbers: distance (km) and time (min), separated by space or tab.

Example input:
```
69.1 76
61.1 69
15.8 17

38.6 30
85.7 64

57.6 75
91.5 99
79.4 84
```



### Output Format
The script generates an Excel (.xlsx) file with the following columns (Rabbit import format):

| ZR NAME* | FROM*  | TO*    | SPEED 1* |
|----------|--------|--------|----------|
| E1-T1    | 0.000  | 69.100 | 54.6     |
| E1-T2    | 0.000  | 61.100 | 53.1     |
| E1-T3    | 0.000  | 15.800 | 55.8     |
| E2-T1    | 0.000  | 38.600 | 77.2     |
| ...      | ...    | ...    | ...      |

Labels are generated as E{stage}-T{index} (e.g., E1-T1 for stage 1, section 1).

Optionally, a second sheet named `FullData` can be included with the following columns for reference:

| ZR NAME* | FROM*  | TO*    | SPEED 1* | Distance_km | Time_min |
|----------|--------|--------|----------|-------------|----------|
| E1-T1    | 0.000  | 69.100 | 54.6     | 69.1        | 76       |
| E1-T2    | 0.000  | 61.100 | 53.1     | 61.1        | 69       |
| ...      | ...    | ...    | ...      | ...         | ...      |


### Usage
Run the script from the command line:
```sh
python txt2rabbit_excel.py -i input.txt -o Rabbit_Import.xlsx
```

**Command-line Flags:**
- `-i`, `--input` (required): Path to the input TXT file.
- `-o`, `--output` (required): Path to the output XLSX file.
- `--include-full` (optional): If set, includes an extra sheet with helper columns (Distance_km, Time_min) in the Excel file.

**Example:**
```sh
python txt2rabbit_excel.py -i input.txt -o Rabbit_Import.xlsx --include-full
```


---


## .NET MAUI Hybrid Application (`rabbit-app/rabbit-maui/`)

This is a cross-platform graphical application built with .NET MAUI. Instead of preparing a TXT file, you enter the stage and segment data directly into the app's user interface. The app then generates a Rabbit-compatible XLSX file from your input.

**Features:**
- Cross-platform (Windows, Android, iOS, Mac)
- Modern UI for entering and previewing stage/segment data
- Exports to the same Rabbit XLSX format as the Python script
- Built-in validation and error handling

**Usage:**
1. Build and run the MAUI app using Visual Studio or the .NET CLI.
2. Use the UI to enter your stages and segments (distance and ideal time).
3. Export the XLSX file for Rabbit import directly from the app.

---

## Customization
You can modify either implementation to adjust the parsing logic for your specific text file format or to change the output columns as needed.



## License
MIT License
