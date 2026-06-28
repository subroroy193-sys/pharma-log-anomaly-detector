import csv
import random
from datetime import datetime, timedelta

users = ["Alice", "Bob", "Charlie", "David", "Eve"]

modules = ["Authentication", "Reports", "Inventory", "Batch Processing", "Audit Trail", "Sample Management"]

events = ["Login", "Logout", "Create", "Update", "Delete", "Export", "Generate Report", "Execute Batch"]

statuses = ["Success", "Failure"]

def generate_random_ip():
    return f"10.1.1.{random.randint(1, 254)}"

def generate_random_duration():
    if random.random() < 0.95:  # 95% chance of being a normal duration
        return random.randint(80, 500)  # Normal duration between 80ms and 500ms
    else:
        return random.randint(5000, 12000)  # Anomalous duration between 5000ms and 12000ms

def generate_log(timestamp):
    status = random.choices(statuses, weights=[95, 5], k=1)[0]  # 95% Success, 5% Failure

    duration = generate_random_duration()

    return {
        "Timestamp": timestamp.strftime("%Y-%m-%d %H:%M:%S"),
        "UserName": random.choice(users),
        "Module": random.choice(modules),
        "Event": random.choice(events),
        "Status": status,
        "DurationMs": duration,
        "IPAddress": generate_random_ip(),
        "Message": ("Operation completed successfully." if status == "Success" else "An error occurred during the operation.")
    }

if __name__ == "__main__":
    output_file = "sample_logs.csv"
    start_time = datetime.now() # Start from 24
    logs = []
    for i in range(100):  # Generate 100 log entries
        log_time = start_time + timedelta(minutes=i)  # Increment by 1 minute
        logs.append(generate_log(log_time))

    with open(output_file, "w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(f, fieldnames=logs[0].keys())
        writer.writeheader()
        writer.writerows(logs)
    
    print(f"{len(logs)} log written to {output_file}.")