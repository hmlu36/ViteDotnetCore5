<template>
    <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
        <div class="card-body">
            <h3 class="card-title">重設密碼</h3>

            <div class="form-group">
                <label>新密碼</label>
                <div class="input-group">
                    <input v-bind:type="[isShowPassword ? 'text' : 'password']" class="form-control form-control-lg" required autofocus />
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" v-on:click="isShowPassword = !isShowPassword">
                            <i :class="[isShowPassword ? 'fas fa-eye' : 'fas fa-eye-slash']"></i>
                        </button>
                    </div>
                </div>
                <div class="form-text text-muted">
                    請確認輸入的密碼強度是安全的
                </div>
            </div>

            <button type="submit" class="btn btn-primary btn-block">重設密碼</button>

        </div>
    </div>
</template>

<script>
    import { ref } from "vue";

    export default {
        setup() {
            const isShowPassword = ref(false);

            return {
                isShowPassword
            }
        }
    }
</script>