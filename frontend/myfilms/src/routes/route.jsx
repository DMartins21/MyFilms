import {Route, BrowserRouter, Routes} from 'react-router-dom'
import Filmes from '../views/Filmes/index.jsx'
import Details from '../views/Filmes/details.jsx'
import Header from '../components/Header/index.jsx'
import App from '../App.jsx'

function RoutesApp()
{
    return(
        <BrowserRouter>
            <Header />
                <Routes>
                    <Route path="/" element={<App />} />
                    <Route path="/filmes" element={<Filmes />} />
                    <Route path="/filmes/:title" element={<Details />} />
                </Routes>
        </BrowserRouter>
    )
}

export default RoutesApp