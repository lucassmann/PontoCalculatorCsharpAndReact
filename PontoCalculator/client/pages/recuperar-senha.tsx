import Link from "next/link";
import styles from "../styles/login.module.css";
import LoginCard from "../src/components/loginCard/loginCard";
import React, { SyntheticEvent, useState } from "react";
import { useRouter } from "next/router";


const RecuperarSenha = () => {
  const [email, setEmail] = useState("");
  const router = useRouter();

  const handleSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    await fetch(`http://localhost:5127/api/Auth/forgot-password?email=${email}`, {
      method: "POST",
    });
    await router.push("/");
  };

    return (
        <div className={styles.background}>
            <LoginCard title="Recupere sua senha">
              <form className={styles.form} onSubmit={handleSubmit}>  
                <input className={styles.input} type="email" placeholder="Seu e-mail" required onChange={e => setEmail(e.target.value)}/>
                <button type="submit" className={styles.button}>Enviar</button>
                <Link href="/cadastro">Ainda não possui conta?</Link>
              </form>
            </LoginCard>
        </div>
    );
};
export default RecuperarSenha;