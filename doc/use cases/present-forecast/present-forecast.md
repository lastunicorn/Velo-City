# Present Forecast

**Actor**: user

**Action**: request a forecast

**Request**:

- EndDate (optional) - The forecast end date.
- ExcludedSprints (optional) - Sprints to be ignored when calculating the average velocity.
- ExcludedTeamMembers (optional) - A list of team member names to to excluded when estimating the velocity.

**Steps**:

1. Create a forecast.

   - 1.1 Get the reference sprint
     - The last in-progress sprint or the last closed sprint if none is in progress.

   - 1.2 Retrieve previous sprints.
   - 1.3 Estimate the velocity for the future sprints.
   - 1.4 Generate future sprints.
   - 1.5 Calculate the total work hours for the future sprints.
   - 1.6 Estimate the SP that can be burn in the future sprints.
   - 1.7 Estimate the SP that can be burn in the future sprints taking into account the velocity penalties.

2. Return the calculated data.

**Errors**:

- If data storage cannot be accessed:
  - `DataAccessException`
- If no in-progress or closed sprint exists:
  - `NoSprintException`
  


**Response**:

- Forecast timespan (start/end date)
- Total work hours.
- Estimated velocity.
- Estimated story points.
- Estimated story points including velocity penalties.
- Sprints details.

**Diagram**:

![Diagram](present-forecast.drawio.png)