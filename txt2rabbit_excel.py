#!/usr/bin/env python3
"""
txt2rabbit_excel.py

Read a plain TXT file containing distances and times for rally stages,
where blank lines separate stages, and generate an Excel (.xlsx) file
in the column format expected by Rabbit import:
  - ZR NAME*
  - FROM*
  - TO*
  - SPEED 1*

Labels will be generated as E{stage}-T{index} (e.g., E1-T1).
All comments and code are in English, as requested.

Example input (TXT):
--------------------
69.1 76
61.1 69
15.8 17

38.6 30
85.7 64

57.6 75
91.5 99
79.4 84

Usage:
------
python txt2rabbit_excel.py -i input.txt -o Rabbit_Import.xlsx
"""

import argparse
import sys
from typing import List, Tuple
import pandas as pd


def parse_txt(lines: List[str]) -> List[List[Tuple[float, float]]]:
    """
    Parse TXT lines into a list of stages.
    Each stage is a list of (distance_km, time_min) tuples.
    Blank lines indicate a new stage.
    """
    stages = []
    current_stage = []

    for raw in lines:
        line = raw.strip()
        # If line is empty -> new stage boundary (if we have accumulated rows)
        if not line:
            if current_stage:
                stages.append(current_stage)
                current_stage = []
            continue

        # Expected "distance time" (space- or tab-separated)
        parts = line.replace(",", ".").split()
        if len(parts) < 2:
            # Skip malformed lines but keep going
            print(f"Warning: Skipping malformed line: {raw!r}", file=sys.stderr)
            continue

        try:
            dist = float(parts[0])
            tmin = float(parts[1])
        except ValueError:
            print(f"Warning: Could not parse numbers in line: {raw!r}", file=sys.stderr)
            continue

        current_stage.append((dist, tmin))

    # Append last stage if not empty
    if current_stage:
        stages.append(current_stage)

    return stages


def build_rows(stages: List[List[Tuple[float, float]]]) -> pd.DataFrame:
    """
    Build a DataFrame with Rabbit import columns:
      - ZR NAME*, FROM*, TO*, SPEED 1*
    Also add helper columns for reference (Distance_km, Time_min).
    """
    rows = []
    for s_idx, stage in enumerate(stages, start=1):
        for t_idx, (dist_km, t_min) in enumerate(stage, start=1):
            if t_min <= 0:
                raise ValueError(f"Time must be > 0. Found {t_min} in E{s_idx}-T{t_idx}.")
            speed_kmh = dist_km / (t_min / 60.0)  # km/h
            rows.append({
                "ZR NAME*": f"E{s_idx}-T{t_idx}",
                "FROM*": 0.000,
                "TO*": round(dist_km, 3),
                "SPEED 1*": round(speed_kmh, 1),
                "Distance_km": dist_km,
                "Time_min": t_min
            })

    df = pd.DataFrame(rows, columns=["ZR NAME*", "FROM*", "TO*", "SPEED 1*", "Distance_km", "Time_min"])
    return df


def save_excel(df: pd.DataFrame, out_path: str, rabbit_only: bool = True) -> None:
    """
    Save the DataFrame to Excel.
    If rabbit_only is True, write only the four Rabbit columns.
    Otherwise include the helper columns as a second sheet.
    """
    cols_rabbit = ["ZR NAME*", "FROM*", "TO*", "SPEED 1*"]

    with pd.ExcelWriter(out_path, engine="xlsxwriter") as writer:
        df[cols_rabbit].to_excel(writer, sheet_name="Sections", index=False)

        if not rabbit_only:
            df.to_excel(writer, sheet_name="FullData", index=False)


def main():
    parser = argparse.ArgumentParser(description="Convert TXT (dist time per line, blank lines between stages) to Rabbit Excel.")
    parser.add_argument("-i", "--input", required=True, help="Path to input TXT file.")
    parser.add_argument("-o", "--output", required=True, help="Path to output XLSX file.")
    parser.add_argument("--include-full", action="store_true", help="Include an extra sheet with helper columns.")
    args = parser.parse_args()

    # Read input TXT
    try:
        with open(args.input, "r", encoding="utf-8") as f:
            lines = f.readlines()
    except Exception as e:
        print(f"Error reading input file: {e}", file=sys.stderr)
        sys.exit(1)

    stages = parse_txt(lines)
    if not stages:
        print("No stages were parsed. Check your TXT formatting.", file=sys.stderr)
        sys.exit(2)

    df = build_rows(stages)

    try:
        save_excel(df, args.output, rabbit_only=not args.include_full)
    except Exception as e:
        print(f"Error writing Excel: {e}", file=sys.stderr)
        sys.exit(3)

    print(f"Done. Wrote {len(df)} sections to: {args.output}")


if __name__ == "__main__":
    main()
