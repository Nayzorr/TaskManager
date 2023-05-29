import React, { useState } from "react";
import {
  Avatar,
  Checkbox,
  CssBaseline,
  FormControlLabel,
  Grid,
  Link,
  Paper,
  TextField,
  Typography,
  Button,
  ThemeProvider,
  createTheme,
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import styles from "./Login.module.css";
import API from "../../api/axios";
import { AUTHENTICATE } from "../../api/url";

const theme = createTheme({
  components: {
    MuiGrid: {
      styleOverrides: {
        root: {
          height: "100vh",
        },
      },
      variants: [
        {
          props: { variant: "image" },
          style: {
            backgroundImage:
              "url(https://images.pexels.com/photos/15286/pexels-photo.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1)",
            backgroundRepeat: "no-repeat",
            backgroundSize: "cover",
            backgroundPosition: "center",
          },
        },
      ],
    },
    MuiAvatar: {
      styleOverrides: {
        root: {
          margin: 1,
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          margin: (3, 0, 2),
        },
      },
    },
  },
});

export default function Login() {
  const [userLogin, setUserLogin] = useState({
    userName: "",
    password: "",
  });
  const [isCorrectUser, setIsCorrectUser] = useState(false);

  const login = () => {
    let user = API.post(AUTHENTICATE, {
      userName: userLogin.userName,
      password: userLogin.password,
    })
      .then((response) => {
        console.log(response.data.data);
        setIsCorrectUser(false);
      })
      .catch((erorr) => {
        if (erorr.response.status === 400) {
          setIsCorrectUser(true);
        }
      });
  };

  return (
    <ThemeProvider theme={theme}>
      <Grid container component="main">
        <CssBaseline />
        <Grid item xs={false} sm={4} md={7} variant="image" />
        <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
          <div className={styles.paper}>
            <Avatar>
              <LockOutlinedIcon />
            </Avatar>
            <Typography component="h1" variant="h5">
              Sign in
            </Typography>
            <form className={styles.form} noValidate>
              <TextField
                error={isCorrectUser}
                variant="outlined"
                margin="normal"
                required
                fullWidth
                id="email"
                label="Email Address"
                name="email"
                autoComplete="email"
                autoFocus
                onChange={(e) =>
                  setUserLogin({ ...userLogin, userName: e.target.value })
                }
              />
              <TextField
                error={isCorrectUser}
                variant="outlined"
                margin="normal"
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
                onChange={(e) =>
                  setUserLogin({ ...userLogin, password: e.target.value })
                }
              />
              <FormControlLabel
                control={<Checkbox value="remember" color="primary" />}
                label="Remember me"
              />
              <Button
                //type="submit"
                fullWidth
                variant="contained"
                color="primary"
                onClick={login}
              >
                Sign In
              </Button>
              <Grid container>
                <Grid item xs>
                  <Link href="#" variant="body2">
                    Forgot password?
                  </Link>
                </Grid>
                <Grid item>
                  <Link href="#" variant="body2">
                    {"Don't have an account? Sign Up"}
                  </Link>
                </Grid>
              </Grid>
            </form>
          </div>
        </Grid>
      </Grid>
    </ThemeProvider>
  );
}
