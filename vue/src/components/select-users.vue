<template>
    <div>
        <Select
            :value="value"
            :remote-method="onSearch"
            :multiple="true"
            :loading="loading"
            filterable
            remote
            @on-change="onChange"
        >
            <Option
                v-for="option in options"
                :key="option.id"
                :value="option.id"
                :disabled="multiple == false && valueArray.length > 0"
            >
                {{ option.userName + "（" + option.fullName + "）" }}
            </Option>
        </Select>
    </div>
</template>
<script lang="ts">
import { Component, Prop } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";

@Component
export default class SelectUsers extends AbpBase {
    @Prop({ default: null })
    readonly value: any;

    @Prop({ default: false })
    readonly multiple: boolean;

    loading: boolean = false;
    initSuccessed: boolean = false;
    valueArray = [];
    options = [];

    async mounted() {
        this.onSearch("");
    }

    async searchUsers(param) {
        return await this.$store.dispatch({
            type: "user/searchList",
            data: param,
        });
    }

    async onSearch(query: string) {
        this.loading = true;
        let param = {
            keyword: query,
            userIds: this.value
                ? this.multiple
                    ? this.value
                    : [this.value]
                : null,
        };
        if (!param.userIds) {
            delete param.userIds;
        }
        this.options = await this.searchUsers(param);
        this.loading = false;
    }

    onChange(value: any) {
        let result = this.multiple ? value : value[value.length - 1];
        this.$emit("input", result);
    }
}
</script>
