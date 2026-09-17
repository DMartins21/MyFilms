import {Route, BrowserRouter, Routes} from 'react-router-dom'
import Filmes from '../pages/Filmes/index.jsx'
import Login from '../pages/Login/index.jsx'
import Details from '../pages/Filmes/details.jsx'
import Error from '../pages/Error/index.jsx'
import Header from '../components/Header/index.jsx'
import App from '../App.jsx'

function RoutesApp()
{
    return(
        <BrowserRouter>
            <Header />
                <Routes>
                    <Route path="/" element={<App />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/filmes" element={<Filmes />} />
                    <Route path="/filmes/:id" element={<Details />} />
                    <Route path='*' element={<Error />} />
                </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp