
async function LoginApi(endpoint = '', options = {}){
    const apiUrl = `http://localhost:5290/${endpoint}`

    try {

        const response = await fetch(apiUrl, options)

        let data = null

        if(response.status !== 204)
        {
            const text = await response.text();
            if(text){
                try{
                    data = JSON.parse(text);
                }catch {
                    data = text;
                }
            }
        }
        if(!response.ok){
            const error = new Error(data?.message || response.statusText)
            error.status = response.status;
            error.data = data;
            throw error;
        }

        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
        throw error
    }
}

export default LoginApi