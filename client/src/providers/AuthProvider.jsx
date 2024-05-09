import { createContext, useEffect, useState} from "react"
import Cookies from "js-cookie"

export const AuthContext = createContext();

export const AuthProvider = ({children}) => {
    const [user, setUser] = useState({});

    useEffect(() => {
        setUser({...user, access_token: Cookies.get("access_token")});
    }, [])

    return <AuthContext.Provider value={[user, setUser]}>{children}</AuthContext.Provider>
}

export default AuthProvider;