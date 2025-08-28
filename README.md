
# txt2rabbit_excel

A Python script to convert plain TXT files containing distances and times for rally stages into Excel spreadsheets, formatted for Rabbit import. Designed for easy data transformation and automation.

## Features
- Reads structured text files and outputs Excel (.xlsx) files
- Simple command-line interface
- Customizable for different text formats
- Generates labels for each section (e.g., E1-T1)
- Supports multiple stages separated by blank lines

## Requirements
- [openpyxl](https://pypi.org/project/openpyxl/) (for Excel file creation)
- [pandas](https://pypi.org/project/pandas/) (for data manipulation)
- [xlsxwriter](https://pypi.org/project/XlsxWriter/) (for Excel writing engine)

## Installation
1. Clone this repository:
   ```sh
   git clone https://github.com/yourusername/txt2rabbit_excel.git
   cd txt2rabbit_excel
   ```
2. Install dependencies:
   ```sh
   pip install -r requirements.txt
   ```

## Input Format
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


## Output Format
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

## Usage
Run the script from the command line:
```sh
python txt2rabbit_excel.py -i input.txt -o Rabbit_Import.xlsx
```

### Command-line Flags
- `-i`, `--input` (required): Path to the input TXT file.
- `-o`, `--output` (required): Path to the output XLSX file.
- `--include-full` (optional): If set, includes an extra sheet with helper columns (Distance_km, Time_min) in the Excel file.

#### Example:
```sh
python txt2rabbit_excel.py -i input.txt -o Rabbit_Import.xlsx --include-full
```

## Customization
You can modify `txt2rabbit_excel.py` to adjust the parsing logic for your specific text file format or to change the output columns as needed.


## License
MIT License
