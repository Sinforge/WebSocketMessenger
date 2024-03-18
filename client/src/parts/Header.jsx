import { AppBar, Toolbar, IconButton, Typography, Stack, Button} from "@mui/material"

export const Header = () => {
    return (
        <AppBar position="statis">
            <Toolbar>
                <IconButton>

                </IconButton>
                <Typography variant="h6" component='div' sx={{flexGrow: 1}}>
                    WebSocketMessenger
                </Typography>
                <Stack direction='row' spacing={2}>
                    <Button color='inherit'>Sign In</Button>
                    <Button color='inherit'>Sign Up</Button>
                </Stack>
            </Toolbar>


        </AppBar>
    )
}