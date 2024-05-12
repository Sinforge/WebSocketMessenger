import { AppBar, Toolbar, Autocomplete, TextField, IconButton, Typography, Stack, Button, Divider} from "@mui/material"
import { useNavigate } from "react-router-dom"
import { useContext, useState } from "react";
import { AuthContext } from "../../providers/AuthProvider";
import DialogStore from "../../store/DialogStore";
import { findUserByUserName } from "../../services/user.service";
import { useEffect } from "react";

export const Header = () => {
    const navigate = useNavigate();
    const [user, setUser] = useContext(AuthContext);
    const userAuthorized = user.access_token !== null && user.access_token !== undefined
    
    const { setOpenedDialog} = DialogStore;
    
    const [searchString, setSearchString] = useState("");
    const [myOptions, setMyOptions] = useState([]);

    async function getData() {
        // fetch data
        if(searchString === undefined || searchString === null || searchString === "") {
            return;
        }
        let data = await findUserByUserName(searchString)
        let mappedData = data.data.map(user => 
            ({
                label: `${user.name} ${user.lastName}`, 
                user : user
            })
        )
        setMyOptions(mappedData);
     }

     useEffect(() => {
        getData()
     }, [searchString])
    
    return (
        <AppBar sx={{ position: "fixed" }}>
            <Toolbar>
                <IconButton>

                </IconButton>
                <Typography variant="h6" component='div' onClick={() => navigate("/")} sx={{ flexGrow: 1 }}>
                    WebSocketMessenger
                </Typography>
                <Autocomplete
                    sx={{ width: '400px' }}
                    autoComplete
                    autoHighlight
                    freeSolo
                    options={myOptions}
                    onInputChange={(e) => setSearchString(e.target.value)}
                    onChange={(e, value) => {
                        if(value !== null || value !== undefined) setOpenedDialog(value.user)}}
                    renderInput={(data) => (
                        <TextField {...data} onClick={() => console.log('clicked with' + data.inputProps.value)} variant="outlined" label="Search Box"/>
                    )}
                />
                <Stack direction='row' spacing={2}>

                    {!userAuthorized && <Button color='inherit' onClick={() => navigate("/signin")}>Sign In</Button>}
                    {!userAuthorized && <Divider orientation="vertical" flexItem/>}
                    {!userAuthorized && <Button color='inherit' onClick={() => navigate("/signup")}>Sign Up</Button>}
                </Stack>
            </Toolbar>
        </AppBar>
    )
}
