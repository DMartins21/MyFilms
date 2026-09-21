import { data } from "react-router-dom"

async function LoginApi(endpoint = '', options = {}){
    const apiUrl = `http://localhost:5290/${endpoint}`

    try {

        const response = await fetch(apiUrl, options)

        const data = await response.json()

        if(!response.ok){
            throw new Error(data.message || response.statusText)
        }

        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
        throw error
    }
}

export default LoginApi