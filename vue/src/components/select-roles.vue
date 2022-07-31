<template>
    <div>
        <Select
            :value="value"
            :multiple="true"
            :loading="loading"
            filterable
            @on-change="onChange"
        >
            <Option
                v-for="option in options"
                :key="option.name"
                :value="option.name"
                :disabled="multiple == false && valueArray.length > 0"
            >
                {{ option.name + "（" + option.displayName + "）" }}
            </Option>
        </Select>
    </div>
</template>
<script lang="ts">
import { Component, Emit, Prop } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";

@Component
export default class SelectRoles extends AbpBase {
    @Prop({ default: null })
    readonly value: any;

    @Prop({ default: false })
    readonly multiple: boolean;

    loading = false;
    valueArray = [];
    initSuccessed = false;
    options = [];

    async mounted() {
        this.options = await this.$store.dispatch({
            type: "user/getRoles",
        });
    }

    onChange(value) {
        let result = this.multiple ? value : value[value.length - 1];
        this.$emit("input", result);
    }
}
</script>
