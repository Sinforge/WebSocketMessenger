import React from "react";
import Typography from "@mui/material/Typography"
import Box from '@mui/material/Box'
import TextField from "@mui/material/TextField"
import Button from "@mui/material/Button"
import { authorizeUser } from "../../services/user.service"
import { useNavigate } from "react-router-dom"
import { AuthContext } from "../../providers/AuthProvider";
import  Cookies from 'js-cookie'
import { useContext } from "react";
import { toast } from "react-toastify";
function Login() {
    const navigate = useNavigate();
    const [user, setUser] = useContext(AuthContext);
    const handleSubmit = async (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);

        let response = await authorizeUser({
            Login : data.get("userName"),
            Password: data.get("password")
        });
        if(response.status === 200) {
            //errorMessage.current = ""
            setUser({...user, access_token :response.data.access_token});
            Cookies.set("access_token", response.data.access_token)
            console.log(response.data);
            navigate("/")

            //console.log(user.access_token)
        }
        else {
            console.log("Error")
            //errorMessage.current = "Incorrect credentials"

        }

        // registerUser(data);
        // navigate("/")
    }
    const notify = () => toast.success('Toast Notification!');

    
    return(
        <Box  component='form' onSubmit={handleSubmit} sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}>
            <Typography>
                Sign In
            </Typography>
            <Box>
                <TextField
                margin="normal"
                required
                fullWidth
                id="userName"
                label="Username"
                name="userName"
                autoComplete="userName"
                autoFocus
                />
                <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
                />
            
                <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
                >
                Sign In
                </Button>
                <Button onClick={notify}>Toast test</Button>
            </Box>
            
            
        </Box>
        
    
    );
}
export default Login;