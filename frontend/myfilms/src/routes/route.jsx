import {Route, BrowserRouter, Routes, Navigate} from 'react-router-dom'
import Filmes from '../pages/Filmes/index.jsx'
import FavFilms from '../pages/Filmes/favFilms.jsx'
import Login from '../pages/Login/login.jsx'
import Register from '../pages/Login/register.jsx'
import Details from '../pages/Filmes/details.jsx'
import Error from '../pages/Error/index.jsx'
import Header from '../components/Header/index.jsx'
import App from '../App.jsx'

function RoutesApp()
{   
    const user = sessionStorage.getItem('user')
    const exToken = sessionStorage.getItem('expirationToken')

    return(
        <BrowserRouter>
            <Header />
                <Routes>
                    <Route path="/" element={<App />} />
                    {!user || Date.now() > new Date(exToken).getTime() ? (
                        <>
                            <Route path="/login" element={<Login />}  />
                            <Route path='/register' element={<Register />} />
                             <Route path='/filmes/favFilms' element={<Navigate to="/login" />} />
                        </>
                    ) : (
                        <>
                        <Route path="/login" element={<Navigate to="/" />} />
                        <Route path='/register' element={<Navigate to="/" />} />
                        <Route path='/filmes/favFilms' element={<FavFilms />} />
                        </>
                    )}

                    <Route path="/filmes" element={<Filmes />} />
                    <Route path="/filmes/:id" element={<Details />} />

                    <Route path='*' element={<Error />} />
                </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp