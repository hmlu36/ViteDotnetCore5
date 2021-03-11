<template>
    <div class="col-sm-9 col-md-7 col-lg-5 mx-auto">
        <div class="card-body">
            <h3 class="card-title">忘記密碼</h3>

            <div class="form-group">
                <label>Email</label>
                <input type="email" class="form-control form-control-lg" required autofocus />
                <div class="invalid-feedback">
                    Email is invalid
                </div>
                <div class="form-text text-muted">
                    按下"重設密碼"後，系統將發送密碼重置信件
                </div>
            </div>

            <button type="submit" class="btn btn-primary btn-block">重設密碼</button>

        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {}
        }
    }
</script>