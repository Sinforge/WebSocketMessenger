import React from "react";
import Typography from "@mui/material/Typography"
import Box from '@mui/material/Box'
import TextField from "@mui/material/TextField"
import Button from "@mui/material/Button"
import Grid from "@mui/material/Grid"
import Checkbox from "@mui/material/FormControlLabel"
import Link from "@mui/material/Link"
import FormControlLabel  from "@mui/material/FormControlLabel"
import { registerUser } from "../../services/user.service"
import { useNavigate } from "react-router-dom"
import { toast } from "react-toastify";
const SignUp = () => {

    const navigate = useNavigate();

    const handleSubmit = async(event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        
        try {
            await registerUser({
                Name: data.get('firstName'),
                Surname: data.get('lastName'),
                Email: data.get('email'),
                Password: data.get('password'),
                UserName: data.get('username')
            });
            navigate("/signin");

        
            // Обработка успешного ответа
            // response содержит данные успешного ответа
        } catch (error) {
            if (error.response && error.response.status === 400) {
                toast.error("User with such email or username already exists")
            } else {
                // Обработка других ошибок
                console.error('Произошла ошибка:', error);
            }
        }
    }
    return(
        //<div>test</div>
        <Box component="form" onSubmit={handleSubmit} sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}>
            <Typography>
                Sign Up
            </Typography>
            <Box sx={{ mt: 3 }}>
                <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                    <TextField
                    autoComplete="given-name"
                    name="firstName"
                    required
                    fullWidth
                    id="firstName"
                    label="First Name"
                    autoFocus
                    />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField
                    required
                    fullWidth
                    id="lastName"
                    label="Last Name"
                    name="lastName"
                    autoComplete="family-name"
                    />
                </Grid>
                <Grid item xs={12}>
                    <TextField
                    required
                    fullWidth
                    id="email"
                    label="Email Address"
                    name="email"
                    autoComplete="email"
                    />
                </Grid>
                <Grid item xs={12}>
                    <TextField
                    required
                    fullWidth
                    name="username"
                    label="Username"
                    type="username"
                    id="username"
                    autoComplete="username"
                    />
                </Grid>
                <Grid item xs={12}></Grid>
                <Grid item xs={12}>
                    <TextField
                    required
                    fullWidth
                    name="password"
                    label="Password"
                    type="password"
                    id="password"
                    autoComplete="new-password"
                    />
                </Grid>
                <Grid item xs={12}>
                    
                </Grid>
                </Grid>
                <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
                >
                Sign Up
                </Button>
                <Grid container justifyContent="flex-end">
                <Grid item>
                    <Link href="#" variant="body2">
                    Already have an account? Sign in
                    </Link>
                </Grid>
                </Grid>
            </Box>
            
            
            
        </Box>
        
    
    );
}
export default SignUp;