import React from 'react';
import { Card, CardContent, ThemeProvider, Typography, createTheme } from '@mui/material';

const theme = createTheme({
    components: {
      MuiCard: {
        styleOverrides: {
          root: {
            maxWidth: 400,
    margin: '0 auto',
    marginTop: 40,
    padding: 20,
          },
        },
      },
    },
  });

export default function User(){

    const user = {
        id: 1,
        userName: 'john_doe',
        email: 'johndoe@example.com',
        firstName: 'John',
        lastName: 'Doe',
        phoneNumber: '123-456-7890',
      };

    return(
        <ThemeProvider theme={theme}>
        <Card >
      <CardContent>
        <Typography variant="h5" component="div">
          User Profile
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>ID:</strong> {user.id}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>Username:</strong> {user.userName}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>Email:</strong> {user.email}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>First Name:</strong> {user.firstName}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>Last Name:</strong> {user.lastName}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          <strong>Phone Number:</strong> {user.phoneNumber}
        </Typography>
      </CardContent>
    </Card>
    </ThemeProvider>
    )
}