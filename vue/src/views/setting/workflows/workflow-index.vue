<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Tabs v-model="tabValue">
                    <TabPane label="首页" name="home">
                        <workflow-definitions></workflow-definitions>
                    </TabPane>
                    <TabPane label="待处理" name="unaudit">
                        <workflow-auditing-list
                            :request="{ auditedMark: false }"
                            v-if="tabValue == 'unaudit'"
                        ></workflow-auditing-list>
                    </TabPane>
                    <TabPane label="已处理" name="audited">
                        <workflow-auditing-list
                            :request="{ auditedMark: true }"
                            v-if="tabValue == 'audited'"
                        ></workflow-auditing-list>
                    </TabPane>
                    <TabPane label="已发起" name="apply">
                        <my-workflows v-if="tabValue == 'apply'"></my-workflows>
                    </TabPane>
                </Tabs>
            </div>
        </Card>
    </div>
</template>

<script lang="ts">
import { Component } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";
import WorkflowDefinitions from "./workflow-definition.vue";
import WorkflowAuditingList from "./workflow-audit-list.vue";
import MyWorkflows from "./my-workflow.vue";

@Component({
    components: {
        WorkflowDefinitions,
        WorkflowAuditingList,
        MyWorkflows,
    },
})
export default class WorkflowIndexs extends AbpBase {
    tabValue = "home";
}
</script>

<style scoped>
</style>