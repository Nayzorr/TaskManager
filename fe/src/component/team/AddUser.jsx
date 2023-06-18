import React, { useState } from "react";
import { TextField, Button } from "@mui/material";
import API from "../../api/axios";

const AddUser = () => {
  const [teamName, setTeamName] = useState("");

  const handleTeamNameChange = (event) => {
    setTeamName(event.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    API.post("Team/CreateTeam", { teamName: teamName });
    console.log("Team Name:", teamName);
    setTeamName(""); // Reset the teamName field
  };

  const [teamId, setTeamId] = useState('');
  const [userName, setUserName] = useState('');

  const handleTeamIdChange = (event) => {
    setTeamId(event.target.value);
  };

  const handleUserNameChange = (event) => {
    setUserName(event.target.value);
  };

  const handleSubmitInvite = (event) => {
    event.preventDefault();
    API.post(`Team/InvitePersonToTeam/10/${userName}`);
    console.log('Team Id:', teamId);
    console.log('User Name:', userName);
    setTeamId('');
    setUserName('');
  };

  return (
    <>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Team Name"
          value={teamName}
          onChange={handleTeamNameChange}
          fullWidth
          margin="normal"
          variant="outlined"
        />
        <Button type="submit" variant="contained" color="primary">
          Create Team
        </Button>
      </form>
      <form onSubmit={handleSubmitInvite}>
        <TextField
          label="Team Name"
          value={teamId}
          onChange={handleTeamIdChange}
          fullWidth
          margin="normal"
          variant="outlined"
        />
        <TextField
          label="User Name"
          value={userName}
          onChange={handleUserNameChange}
          fullWidth
          margin="normal"
          variant="outlined"
        />
        <Button type="submit" variant="contained" color="primary">
          Invite User
        </Button>
      </form>
    </>
  );
};

export default AddUser;
