<template>
    <div class="valign-wrapper login-form">
        <div class="col card hoverable s10 pull-s1 m6 pull-m3 l4 pull-l4">
            <form @submit.prevent="login">
                <div class="card-content">
                    <span class="card-title">帳號登入</span>
                    <div class="row">
                        <div class="input-field col s12">
                            <i class="material-icons prefix">account_circle</i>
                            <input id="account" type="text" v-model="loginUser.account">
                            <label for="account">帳號</label>
                        </div>
                        <div class="input-field col s12">
                            <i class="material-icons prefix">lock_outline</i>
                            <input :type="showPassword ? 'text' : 'password'" id="inputPassword" v-model="loginUser.password">
                            <label for="inputPassword">密碼</label>
                            <span class="field-icon" @click="showPassword = !showPassword">
                                <span class="material-icons">
                                    {{showPassword ? 'visibility' : 'visibility_off'}}
                                </span>
                            </span>
                        </div>
                    </div>

                    <div class="card-action right-align">
                        <input type="reset" id="reset" class="btn-flat grey-text waves-effect">
                        <button class="btn waves-effect indigo" type="submit">登入</button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import { reactive } from "vue";
    import store from "../../store";

    export default {
        data() {
            return {
                showPassword: false
            }
        },
        setup() {
            const loginUser = reactive({ account: "", password: "" });
            const login = () => {
                //console.log(JSON.stringify(loginUser));
                store.dispatch("login", loginUser);
            }

            // include google reCaptcha V3
            let tag = document.createElement("script");
            tag.setAttribute("src", "https://www.google.com/recaptcha/api.js?render=6Lfq3HsaAAAAAAJd-q-NUR1enFjEprK4JkhrGioT");
            document.head.appendChild(tag);

            return {
                loginUser,
                login,
            }
        }
    }
</script>

<style scoped>

    span.field-icon {
        float: right;
        position: absolute;
        right: 10px;
        top: 10px;
        cursor: pointer;
        z-index: 2;
    }

    .login-form {
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        height: 100vh;
    }

</style>
