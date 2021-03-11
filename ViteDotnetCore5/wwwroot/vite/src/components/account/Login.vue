<template>
    <div class="container">
        <div class="row">
            <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
                <div class="card card-signin my-5">
                    <div class="card-body">
                        <h5 class="card-title text-center">帳號登入</h5>
                        <div class="form-label-group">
                            <input class="form-control form-control-lg" id="account" placeholder="帳號" autofocus v-model="loginUser.account">
                            <label for="account">帳號</label>
                        </div>

                        <div class="form-label-group">
                            <input type="password" id="inputPassword" class="form-control form-control-lg" placeholder="密碼" v-model="loginUser.password" data-eye>
                            <i class="far fa-eye form-control-icon"></i>
                            <label for="inputPassword">密碼</label>
                        </div>

                        <div class="custom-control custom-checkbox mb-3">
                            <input type="checkbox" class="custom-control-input" id="customCheck1">
                            <label class="custom-control-label" for="customCheck1">Remember password</label>
                        </div>
                        <button class="btn btn-primary btn-block text-uppercase" v-on:click="login">登入</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { reactive } from "vue";
    import store from "../../store";


    export default {
        setup() {
            const loginUser = reactive({ account: "", password: "" });
            const login = () => {
                //console.log(JSON.stringify(loginUser));
                store.dispatch("login", loginUser);
            }

            function onSubmit() {
                //console.log(JSON.stringify(loginUser));
                store.dispatch("login", loginUser);
            }

            return {
                loginUser,
                login,
            }
        }
    }
</script>

<style scoped>

    body {
        background: #007bff;
        background: linear-gradient(to right, #0062E6, #33AEFF);
    }

    .card-signin {
        border: 0;
        border-radius: 1rem;
        box-shadow: 0 0.5rem 1rem 0 rgba(0, 0, 0, 0.1);
    }

        .card-signin .card-title {
            margin-bottom: 2rem;
            font-weight: 300;
            font-size: 1.5rem;
        }

        .card-signin .card-body {
            padding: 2rem;
        }

    .form-signin {
        width: 100%;
    }

        .form-signin .btn {
            font-size: 80%;
            border-radius: 5rem;
            letter-spacing: .1rem;
            font-weight: bold;
            padding: 1rem;
            transition: all 0.2s;
        }

    .form-label-group {
        position: relative;
        margin-bottom: 1rem;
    }


        .form-label-group > input,
        .form-label-group > label {
            left: 20px;
            /*padding: var(--input-padding-y) var(--input-padding-x);*/
        }

        .form-label-group input {
            border-radius: 2rem;
        }

        .form-label-group > label {
            position: absolute;
            top: 10px;
            display: block;
            width: 100%;
            margin-bottom: 0;
            /* Override default `<label>` margin */
            /*line-height: 1.5;*/
            color: #495057;
            border: 1px solid transparent;
            border-radius: .25rem;
            transition: all .1s ease-in-out;
        }

        .form-label-group input::-webkit-input-placeholder {
            color: transparent;
        }

        .form-label-group input:-ms-input-placeholder {
            color: transparent;
        }

        .form-label-group input::-ms-input-placeholder {
            color: transparent;
        }

        .form-label-group input::-moz-placeholder {
            color: transparent;
        }

        .form-label-group input::placeholder {
            color: transparent;
        }

        .form-label-group input:not(:placeholder-shown) {
            padding-top: calc(var(--input-padding-y) + var(--input-padding-y) * (2 / 3));
            padding-bottom: calc(var(--input-padding-y) / 3);
        }

            .form-label-group input:not(:placeholder-shown) ~ label {
                padding-top: calc(var(--input-padding-y) / 3);
                padding-bottom: calc(var(--input-padding-y) / 3);
                top: 0;
                font-size: 12px;
                color: #777;
            }


    /* Fallback for Edge
    -------------------------------------------------- */

    @supports (-ms-ime-align: auto) {
        .form-label-group > label {
            display: none;
        }

        .form-label-group input::-ms-input-placeholder {
            color: #777;
        }
    }

    /* Fallback for IE
    -------------------------------------------------- */

    @media all and (-ms-high-contrast: none), (-ms-high-contrast: active) {
        .form-label-group > label {
            display: none;
        }

        .form-label-group input:-ms-input-placeholder {
            color: #777;
        }
    }
</style>
