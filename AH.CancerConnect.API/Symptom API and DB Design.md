
## Database Tables

### 1. Symptoms Table
Stores the master list of symptoms that patients can report.

| Column Name | Data Type |
|-------------|-----------|
| Id | int |
| Name | nvarchar(100) |
| SeverityType | int |

**Sample Data:**
```
Id | Name                                    | SeverityType
1  | Anxiety                                | 0 (MildModerateSevere)
2  | Appetite Loss                          | 0 (MildModerateSevere)
3  | Bleeding                               | 1 (YesNo)
12 | Pain                                   | 2 (Scale1To10)
19 | Level of Emotional Distress            | 2 (Scale1To10)
```

### 2. SymptomAssessments Table
Stores one record per patient assessment session, containing date and notes.

| Column Name | Data Type |
|-------------|-----------|
| Id | int |
| PatientId | int |
| Date | datetime2 |
| Notes | nvarchar(1000) |

**Sample Data:**
```
Id | PatientId | Date                    | Notes
1  | 123      | 2025-09-22 10:30:00    | Patient reports feeling better today
2  | 123      | 2025-09-20 14:15:00    | Side effects from new medication
3  | 456      | 2025-09-22 09:00:00    | First assessment after surgery
```

### 3. SymptomEntries Table
Stores individual symptom responses within each assessment.

| Column Name | Data Type |
|-------------|-----------|
| Id | int |
| SymptomAssessmentId | int |
| SymptomId | int |
| Severity | nvarchar(50) |

**Sample Data:**
```
Id | SymptomAssessmentId | SymptomId | Severity
1  | 1                  | 1         | Mild
2  | 1                  | 2         | Moderate
3  | 1                  | 3         | No
4  | 1                  | 12        | 7
5  | 2                  | 1         | Severe
6  | 2                  | 2         | Mild
7  | 2                  | 3         | Yes
```



## API Endpoints

### GET /api/v1/symptoms
Returns all symptoms with their possible severity options based on SeverityType.

**Response JSON:**
```json
[
  {
    "id": 1,
    "name": "Anxiety",
    "severityType": "MildModerateSevere",
    "severityOptions": ["Mild", "Moderate", "Severe"]
  },
  {
    "id": 3,
    "name": "Bleeding",
    "severityType": "YesNo",
    "severityOptions": ["No", "Yes"]
  },
  {
    "id": 12,
    "name": "Pain",
    "severityType": "Scale1To10",
    "severityOptions": ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"]
  }
]
```

### POST /api/v1/symptoms/assessments
Creates a new assessment with all symptom entries in a single transaction.

**Request JSON:**
```json
{
  "patientId": 123,
  "date": "2025-09-22T10:30:00Z",
  "notes": "Patient reports feeling better today after medication adjustment",
  "entries": [
    { "symptomId": 1, "severity": "Mild" },
    { "symptomId": 3, "severity": "No" },
    { "symptomId": 12, "severity": "7" },
    { "symptomId": 19, "severity": "4" }
  ]
}
```

**Response JSON:**
```json
{
  "success": true,
  "assessmentId": 42,
  "count": 19
}
```