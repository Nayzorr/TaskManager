import { useState } from "react";
import { CHANGE_FRIEND_STATUS } from "../../api/url";
import API from "../../api/axios";
import { Button, TextField, Typography } from "@mui/material";

export default function Search(){
    const [userName, setUserName] = useState(null);

    const invite = () => {
        let user = API.post(CHANGE_FRIEND_STATUS, {
            userNameToChangeStatus: userName,
            friendStatus: 1
        })
          .then((response) => {
            console.log(response.data.data);
          })
          .catch((erorr) => {
            if (erorr.response.status === 400) {
            }
          });
      };

    return(
        <div>
        <Typography component="h1" variant="h5">
              Search
            </Typography>
              <TextField
                variant="outlined"
                margin="normal"
                required
                fullWidth
                id="User"
                label="User Name"
                name="user"
                autoFocus
                onChange={(e) => {
                    setUserName(e.target.value);
                }}
              />
              <Button
                //type="submit"
                fullWidth
                variant="contained"
                color="primary"
                onClick={invite}
              >
                Send Invite
              </Button>
              </div>
    )
}