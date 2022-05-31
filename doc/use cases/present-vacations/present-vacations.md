# Present Vacations

**Actor**: user

**Action**: request vacations for a team member

**Request**:

- TeamMemberName (optional) - The name of the team member for which to return the vacations. It may be just part of the name. Vacation are returned for all matching team members.

## If TeamMemberName is provided

**Steps**:

1. Retrieve, from the data storage, all the team members those name contain the specified text (TeamMemberName).
2. Sort them in ascending order by last employment date.
2. Create the vacation list for each team member.

**Errors**:

- Database cannot be opened
  - `DataAccessException`

**Response**:

- The vacations for all matching team members.
- The date for which the response is calculated.

**Diagram**:

![Diagram](present-vacation-by-name.drawio.png)

## If Date is provided

**Steps**:

1. Retrieve, from the data storage, the team members active in the specified date interval.
2. Sort them in ascending order by last employment date.

**Errors**:

- Database cannot be opened
  - `DataAccessException`

**Response**:

- The list of team members ordered by last employment date.
- The date interval for which the response is calculated.

**Diagram**:

![Diagram](present-team-by-date.drawio.png)

## If SprintNumber is provided

**Steps**:

1. Retrieve, from the data storage, the sprint with the specified number.
2. Retrieve, from the data storage, the team members active in the sprint's date interval.
3. Sort them in ascending order by last employment date.

**Errors**:

- Database cannot be opened
  - `DataAccessException`
- Sprint with specified number does not exist.
  - `SprintDoesNotExistException`


**Response**:

- The list of team members ordered by last employment date.
- The sprint's number.
- The sprint's date interval.

**Diagram**:

![Diagram](present-team-by-sprint.drawio.png)

## If request is empty

**Steps**:

1. Retrieve, from the data storage, the current sprint.
2. Retrieve, from the data storage, the team members active in the sprint's date interval.
3. Sort them in ascending order by last employment date.

**Errors**:

- Database cannot be opened
  - `DataAccessException`
- There is no sprint in progress.
  - `NoSprintException`

**Response**:

- The list of team members ordered by last employment date.
- The sprint's number.
- The date interval of the sprint.

**Diagram**:

![Diagram](present-team-by-current-sprint.drawio.png)