import '../User/css/profile.css'
import Api from '../../services/userApi';
import { useEffect, useState } from 'react';

function MyUser(){

    const [user, setUser] = useState([]);

    useEffect(() => {
        try{
            async function getUser()
            {
                const data = await Api('MyUser', {
                    method: 'GET',
                    headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                    }
                });
                setUser(data)
            }

            getUser();
        }catch(e){
            console.error(e)
        }
    }, [setUser])

    return(
        <>
            <section className='info-user'>
                <img className='profile-picture'
                src={user.profilePictureUrl} />
                
                <article >
                    <h2 className='title'>Meus Dados</h2>

                    <dl className='data-list'>
                        <div className='data-item'>
                            <dt>Nome</dt>
                            <dd className='fullName-user'>{user.name}</dd>
                        </div>
                        <div className='data-item'>
                            <dt>Sobrenome</dt>
                            <dd>{user.lastName}</dd>
                        </div>
                        
                    </dl>
                </article>
                
                <article className='card card-birth'>
                    <h2 className='title'>Nascimento</h2>
                    <dl className='data-list'>
                        <div className='data-item'>
                            <dt>Data</dt>
                            <dd className='birthDate-info'>{new Date(user.birthDate).toLocaleDateString('pt-br', {timeZone: 'UTC'})}</dd>
                        </div>
                    </dl>
                </article>

            </section>
        </>
    )

}


export default MyUser;