import { useEffect, useState } from "react";
import { CHANGE_FRIEND_STATUS } from "../../api/url";
import API from "../../api/axios";
import { Autocomplete, Button, TextField, Typography } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";

const columns = [
  { field: "userName", headerName: "Username", width: 150 },
  { field: "email", headerName: "Email", width: 250 },
  { field: "firstName", headerName: "First Name", width: 130 },
  { field: "lastName", headerName: "Last Name", width: 130 },
  { field: "phoneNumber", headerName: "Phone Number", width: 150 },
];

export default function Search() {
  const [userName, setUserName] = useState(null);
  const [options, setOptions] = useState([]);
  const [, setInputValue] = useState("");
  const [pendingFriends, setPendingFriends] = useState([]);

  useEffect(() => {
    getPendingFriends();
  }, []);

  const invite = () => {
    let user = API.post(CHANGE_FRIEND_STATUS, {
      userNameToChangeStatus: userName,
      friendStatus: 1,
    })
      .then((response) => {
        console.log(response.data.data);
        getPendingFriends();
        setUserName(null);
      })
      .catch((erorr) => {
        if (erorr.response.status === 400) {
        }
      });
  };

  const handleChange = (event) => {
    if (event.target.value.length > 2) {
      setInputValue(event.target.value);
      const url = `http://localhost:7038/taskmanagerapi/api/User/SearchUsersByUserName?stringToSearch=${event.target.value}`;
      API.get(url)
        .then(function (response) {
          // handle success
          const { status, data } = response;
          if (status === 200) {
            console.log(data.data);
            setOptions(data.data);
          }
        })
        .catch(function (error) {
          // handle error
          console.log(error);
        });
    }
  };

  const getPendingFriends = () => {
    API.get("User/GetPendingFriendsLists")
      .then((response) => {
        console.log(response.data.data);
        setPendingFriends(response.data.data);
      })
      .catch((erorr) => {
        if (erorr.response.status === 400) {
        }
      });
  };
  return (
    <div>
      <Typography component="h1" variant="h5">
        Search
      </Typography>
      <Autocomplete
        disablePortal
        id="combo-box-demo"
        onChange={(event, newValue) => {
          setUserName(newValue);
        }}
        options={options.map((option) => option.userName)}
        sx={{ width: 300 }}
        renderInput={(params) => (
          <TextField
            {...params}
            variant="outlined"
            margin="normal"
            required
            fullWidth
            id="User"
            label="User Name"
            name="user"
            autoFocus
            onChange={handleChange}
          />
        )}
      />
      <Button fullWidth variant="contained" color="primary" onClick={invite} disabled={!userName}>
        Send Invite
      </Button>
      <div style={{ height: 240, width: "100%" }}>
        <p style={{fontSize: 18}}>users to whom the invitation was sent</p>
        <DataGrid
          rows={pendingFriends}
          columns={columns}
          pageSize={5}
          disableSelectionOnClick
        />
      </div>
    </div>
  );
}
