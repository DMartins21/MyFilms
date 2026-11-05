import {Route, BrowserRouter, Routes} from 'react-router-dom'
import ProtectRoute from './protectRoute.jsx'

//Home
import App from '../App.jsx'

//Components
import Header from '../components/Header/index.jsx'
import Footer from '../components/Footer/footer.jsx'
import Error from '../pages/Error/index.jsx'
import Unauthorized from '../pages/Unauthorized/index.jsx'


//Filmes
import Filmes from '../pages/Filmes/index.jsx'
import SubmitFilm from '../pages/Filmes/submitFilm.jsx'
import Details from '../pages/Filmes/details.jsx'

//Login
import Login from '../pages/Login/login.jsx'
import Register from '../pages/Login/register.jsx'
import Logout from '../pages/Login/logout.jsx'

//User
import MyUser from '../pages/User/myUser.jsx'
import CreateUser from '../pages/User/createUser.jsx'
import ModifyUser from '../pages/User/modifyUser.jsx'
import MyFavFilms from '../pages/User/myFavFilms.jsx'

//Posts
import Posts from '../pages/Posts/index.jsx'
import PostDetails from '../pages/Posts/details.jsx'


function RoutesApp()
{   

    return(
        <BrowserRouter>
            <Header />
                <Routes>

                    <Route path='/login' element={< Login />}  />
                    <Route path='/register' element={< Register />} />
                    <Route path='/filmes' element={< Filmes />} />
                    <Route path='/filmes/:id' element={< Details />} />
                    <Route path='/posts' element={< Posts />} />
                    <Route path='/posts/:id' element={< PostDetails />} />
                    <Route path='/' element={< App />} />

                    <Route element={<ProtectRoute allowedRoles={['Admin', 'User']} />}>

                        <Route path='/user/favs' element={< MyFavFilms />} />
                        <Route path='/user/myUser' element={< MyUser />} />
                        <Route path="/user/createUser" element={< CreateUser />} />
                        <Route path='/user/modify' element={< ModifyUser />} />
                        <Route path='/logout' element={< Logout />} />

                    </Route>
                    <Route element={<ProtectRoute allowedRoles={['Admin', 'SuperAdmin']} />}>
                        <Route path='/filmes/submit' element={< SubmitFilm />} />
                    </Route>

                    <Route path='/unauthorized' element={< Unauthorized />} />
                    <Route path='*' element={< Error />} />
                    
                </Routes>
                <Footer />
        </BrowserRouter>
    )
}

export default RoutesApp