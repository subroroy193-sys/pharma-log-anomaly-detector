import argparse
import json
import pandas as pd
from sklearn.ensemble import IsolationForest


def load_data(csv_path: str) -> pd.DataFrame:
    return pd.read_csv(csv_path)


def engineer_features(df: pd.DataFrame) -> pd.DataFrame:
    status_map = {
        "Success": 0,
        "Failure": 1,
        "Warning": 2
    }

    module_map = {
        "Authentication": 0,
        "Reports": 1,
        "Inventory": 2,
        "Batch Processing": 3,
        "Audit Trail": 4,
        "Sample Management": 5
    }

    event_map = {
        "Login": 0,
        "Logout": 1,
        "Create": 2,
        "Update": 3,
        "Delete": 4,
        "Export": 5,
        "Generate Report": 6,
        "Execute Batch": 7
    }

    df["StatusCode"] = df["Status"].map(status_map).fillna(0)
    df["ModuleCode"] = df["Module"].map(module_map).fillna(0)
    df["EventTypeCode"] = df["Event"].map(event_map).fillna(0)

    return df


def detect_anomalies(df: pd.DataFrame) -> pd.DataFrame:
    features = df[["DurationMs", "StatusCode", "ModuleCode", "EventTypeCode"]].copy()
    features["DurationMs"] = features["DurationMs"].fillna(features["DurationMs"].median())

    model = IsolationForest(
        n_estimators=100,
        contamination=0.05,
        random_state=42
    )

    df["Prediction"] = model.fit_predict(features)
    df["AnomalyScore"] = model.decision_function(features)

    return df


def build_output(df: pd.DataFrame) -> list:
    anomalies = df[df["Prediction"] == -1].copy()

    anomalies["Prediction"] = anomalies["Prediction"].map({
        1: "Normal",
        -1: "Anomaly"
    })

    output_columns = [
        "LogEntryId",
        "Timestamp",
        "UserName",
        "Module",
        "Event",
        "Status",
        "DurationMs",
        "Prediction",
        "AnomalyScore"
    ]

    return anomalies[output_columns].to_dict(orient="records")


def main():
    parser = argparse.ArgumentParser(description="Pharma log anomaly detector")
    parser.add_argument("csv_path", help="Path to the input CSV file")
    args = parser.parse_args()

    df = load_data(args.csv_path)
    print(df.columns.tolist())
    df = engineer_features(df)
    df = detect_anomalies(df)
    output = build_output(df)

    print(json.dumps(output, indent=2))


if __name__ == "__main__":
    main()