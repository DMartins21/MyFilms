import { Link } from "react-router-dom";
import "./css/styleFooter.css";

export default function Footer() {
    const year = new Date().getFullYear();

    return (
        <footer className="footer">
            <div className="footer-strip" aria-hidden="true" />

            <div className="footer-content">
                <div className="footer-brand">
                    <Link to="/" className="footer-logo">MyFilms</Link>
                    <p>Seu catálogo pessoal de filmes: cadastre, organize e redescubra o que você já assistiu.</p>
                </div>

                <nav className="footer-nav" aria-label="Rodapé">
                    <h3>Navegação</h3>
                    <ul>
                        <li><Link to="/">Início</Link></li>
                    </ul>
                </nav>

                <div className="footer-nav">
                    <h3>Contato</h3>
                    <ul>
                        <li><a href="mailto:davi_martins2009@outlook.com">Contato</a></li>
                        <li><a href="https://github.com/DMartins21" target="_blank" rel="noreferrer">GitHub</a></li>
                    </ul>
                </div>
            </div>

            <p className="footer-copy">© {year} MyFilms. Todos os direitos reservados.</p>
        </footer>
    );
}