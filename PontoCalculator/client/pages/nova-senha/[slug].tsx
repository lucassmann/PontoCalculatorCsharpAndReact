import Link from "next/link";
import styles from "../../styles/login.module.css";
import LoginCard from "../../src/components/loginCard/loginCard";
import React, { SyntheticEvent, useEffect, useState } from "react";
import { useRouter } from "next/router";
import { GetServerSideProps } from "next";

export default function NovaSenha ({ token })  {
  const [password, setPassword] = useState("");
  const router = useRouter();

  const handleSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    console.log ({token, password});
    await fetch("http://localhost:5127/api/Auth/reset-password", {
    method: "POST",
    headers: {'Content-Type': 'application/json'},
    body: JSON.stringify({
        "passwordResetToken": token,
        "newPassword": password,
    }),
    });
    await router.push("/");
  };

    return (
        <div className={styles.background}>
            <LoginCard title="Cria sua nova senha">
              <form className={styles.form} onSubmit={handleSubmit}>  
                <input className={styles.input} type="password" placeholder="Sua nova senha" required onChange={e => setPassword(e.target.value)}/>
                <button type="submit" className={styles.button}>Entrar</button>
              </form>
            </LoginCard>
        </div>
    );
};

export const getServerSideProps: GetServerSideProps = async (ctx) => {
    const token = ctx.params?.slug;
    const response = await fetch(`http://localhost:5127/api/Auth/validate-token?passwordResetToken=${token}`, {
        method: "GET",
    });
    if (response.status !== 200) {
        console.log("token invalido");
        return {
            notFound: true,
        };
    }
    
    return {
        props: { token }
    };
}
