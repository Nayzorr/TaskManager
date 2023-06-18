import React, { useState } from "react";
import { Button, TextField, Grid } from "@mui/material";

const data = {
  id: 1,
  creatorId: 3,
  teamName: "BestTeam",
  dateCreated: "2023-02-18T11:19:04.967",
  teamMembers: [
    {
      id: 2,
      userName: "JohnDoe",
      email: "john.doe@example.com",
      firstName: "John",
      lastName: "Doe",
      phoneNumber: "+1 123-456-7890",
    },
    {
      id: 10,
      userName: "Yurii",
      email: "yuramukomel17@gmail.com",
      firstName: "Yurii",
      lastName: "Mukomel",
      phoneNumber: "0967146151",
    },
    {
      id: 2,
      userName: "JaneSmith",
      email: "jane.smith@example.com",
      firstName: "Jane",
      lastName: "Smith",
      phoneNumber: "+1 987-654-3210",
    },
    {
      id: 2,
      userName: "AlexJohnson",
      email: "alex.johnson@example.com",
      firstName: "Alex",
      lastName: "Johnson",
      phoneNumber: "+1 555-123-4567",
    },
    {
      id: 2,
      userName: "EmilyBrown",
      email: "emily.brown@example.com",
      firstName: "Emily",
      lastName: "Brown",
      phoneNumber: "+1 789-456-1230",
    },
    {
      id: 2,
      userName: "MichaelWill",
      email: "michael.wilson@example.com",
      firstName: "Michael",
      lastName: "Will",
      phoneNumber: "+1 234-567-8901",
    },
  ],
};

export default function AboutTeam() {
  const [teamName, setTeamName] = useState(data.teamName);

  const handleChangeTeamName = () => {
    // Perform any necessary actions with the updated teamName
    console.log("Updated Team Name:", teamName);
  };

  const handleDeleteUser = (userId) => {
    // Perform any necessary actions with the userId
    console.log("Deleted User with ID:", userId);
  };

  return (
    <div>
      <h2>Team Information</h2>
      <TextField
        label="Team Name"
        value={teamName}
        onChange={(e) => setTeamName(e.target.value)}
        fullWidth
        margin="normal"
        variant="outlined"
      />
      <Button
        variant="contained"
        color="primary"
        onClick={handleChangeTeamName}
      >
        Change Team Name
      </Button>
      <div style={{ width: "100%", padding: 40 }}>
        <Grid container spacing={2}>
          {data.teamMembers.map((member) => (
            <Grid item key={member.id} xs={12} sm={6} md={4} lg={3}>
              <div>
                {member.userName} - {member.firstName} {member.lastName}{" "}
                <Button
                  variant="contained"
                  color="secondary"
                  onClick={() => handleDeleteUser(member.id)}
                >
                  Delete User
                </Button>
              </div>
            </Grid>
          ))}
        </Grid>
      </div>
    </div>
  );
}
