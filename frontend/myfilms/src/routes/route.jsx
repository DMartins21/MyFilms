import {Route, BrowserRouter, Routes, Navigate} from 'react-router-dom'
import Filmes from '../pages/Filmes/index.jsx'
import FavFilms from '../pages/Filmes/favFilms.jsx'
import Login from '../pages/Login/login.jsx'
import SubmitFilm from '../pages/Filmes/submitFilm.jsx'
import Register from '../pages/Login/register.jsx'
import MyUser from '../pages/User/myUser.jsx'
import CreateUser from '../pages/User/createUser.jsx'
import ModifyUser from '../pages/User/modifyUser.jsx'
import Details from '../pages/Filmes/details.jsx'
import Logout from '../pages/Login/logout.jsx'
import Error from '../pages/Error/index.jsx'
import Unauthorized from '../pages/Unauthorized/index.jsx'
import Header from '../components/Header/index.jsx'
import App from '../App.jsx'
import Footer from '../components/Footer/footer.jsx'
import MyFavFilms from '../pages/User/myFavFilms.jsx'


function isTokenExpired(token){
        if(!token){
            return true
        }
        if(Date.now() > new Date(token).getTime()){
            return true
        }

        return false
    }


function RoutesApp()
{   
    const user = sessionStorage.getItem('user')
    const exToken = sessionStorage.getItem('expirationToken')

    return(
        <BrowserRouter>
            <Header />
                <Routes>
                    <Route path="/" element={< App />} />
                    {!user || isTokenExpired(exToken) ? (
                        <>  
                            <Route path="/login" element={< Login />}  />
                            <Route path='/register' element={< Register />} />
                             <Route path='/filmes/favFilms' element={< Navigate to="/login" />} />
                        </>
                    ) : (
                        <>
                        <Route path="/login" element={< Navigate to="/" />} />
                        <Route path='/register' element={< Navigate to="/" />} />
                        <Route path='/filmes/favFilms' element={< FavFilms />} />
                        <Route path='/client/favs' element={< MyFavFilms />} />
                        <Route path='/client/myUser' element={< MyUser />} />
                        <Route path="/client/createUser" element={< CreateUser />} />
                        <Route path='/client/modify' element={< ModifyUser />} />
                        <Route path='/filmes/submitFilm' element={< SubmitFilm />} />
                        <Route path='/logout' element={< Logout />} />
                        </>
                    )}

                    <Route path="/filmes" element={< Filmes />} />
                    <Route path="/filmes/:id" element={< Details />} />
                    <Route path='/unauthorized' element={< Unauthorized />} />


                    <Route path='*' element={< Error />} />
                </Routes>
                <Footer />
        </BrowserRouter>
    )
}

export default RoutesApp