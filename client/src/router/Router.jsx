import { Routes, Route, BrowserRouter} from "react-router-dom"
import Login from "../pages/Login/Login"
import Main from "../pages/Main/Main"
import SignUp from "../pages/SignUp/SignUp";
import { Header } from "../parts/Header/Header";
import { useNavigate } from "react-router-dom"

import { useContext } from "react";
import { AuthContext } from "../providers/AuthProvider";
const Router = () => {  
    //const navigate = useNavigate();
    const [user, setUser] = useContext(AuthContext);
    const userAuthorized = user.access_token !== null && user.access_token !== undefined

    return(
        <BrowserRouter>
            <Header/>
            <Routes>
                {/* <Route path="/find" element={<}/> */}
                {userAuthorized && (<Route path="/" element={<Main/>}/>)}
                    <Route path="/signup" element={<SignUp/>}/>
                    <Route path="/signin" element={<Login/>}/>
            </Routes>
        </BrowserRouter>
    )
}
export default Router;