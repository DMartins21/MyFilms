async function LoginApi(endpoint = '', options = {}){
    const apiUrl = `http://localhost:5290/${endpoint}`

    try {

        const response = await fetch(apiUrl, options)

        if(!response.ok){
            throw new Error(response.statusText)
        }

        const data = await response.json()
        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
        throw error
    }
}

export default LoginApi